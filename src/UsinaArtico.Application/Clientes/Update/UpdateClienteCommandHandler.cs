using Microsoft.EntityFrameworkCore;
using UsinaArtico.Application.Abstractions.Data;
using UsinaArtico.Application.Abstractions.Messaging;
using UsinaArtico.Application.Abstractions.Services;
using UsinaArtico.Application.Clientes.Shared;
using UsinaArtico.Domain.Clientes;
using UsinaArtico.Domain.Contratos;
using UsinaArtico.Domain.Enderecos;
using UsinaArtico.SharedKernel;

namespace UsinaArtico.Application.Clientes.Update;

/// <summary>
/// Handler responsável por atualizar um cliente existente no sistema.
/// </summary>
internal sealed class UpdateClienteCommandHandler(
    IApplicationDbContext context,
    ICidadeResolverService cidadeResolverService)
    : ICommandHandler<UpdateClienteCommand>
{
    public async Task<Result> Handle(UpdateClienteCommand command, CancellationToken cancellationToken)
    {
        // 1. Carregar cliente com seus endereços e contratos
        var cliente = await context.Clientes
            .Include(c => c.Enderecos)
                .ThenInclude(e => e.Contrato)
            .FirstOrDefaultAsync(c => c.Id == command.Id, cancellationToken);

        if (cliente is null)
        {
            return Result.Failure(ClienteErrors.NotFound(command.Id));
        }

        // 2. Validar Campos Unicos
        var emailResult = await ValidateEmailAsync(cliente, command.Email, cancellationToken);
        if (emailResult.IsFailure) return emailResult;

        var documentoResult = await ValidateDocumentoAsync(cliente, command.Documento, cancellationToken);
        if (documentoResult.IsFailure) return documentoResult;

        var codigoClienteResult = await ValidateCodigoClienteAsync(cliente, command.CodigoCliente, cancellationToken);
        if (codigoClienteResult.IsFailure) return codigoClienteResult;

        var enderecosResult = await ValidateEnderecosAsync(cliente, command.Enderecos, cancellationToken);
        if (enderecosResult.IsFailure) return enderecosResult;

        // 3. Preparar dados para atualização
        var email = Email.Create(command.Email).Value;
        var (_, cpf, cnpj) = DocumentoValidator.ValidarEIdentificarTipo(command.Documento).Value;
        var tipoPessoa = cpf is not null ? TipoPessoa.Fisica : TipoPessoa.Juridica;

        // 4. Atualizar dados do cliente
        cliente.Update(
            command.Nome,
            email,
            command.Telefone,
            command.CodigoCliente,
            tipoPessoa,
            cpf,
            cnpj
        );

        // 5. Processar endereços (remover, atualizar e criar)
        await ProcessarEnderecosAsync(command.Enderecos, cliente, cancellationToken);

        // 6. Salvar todas as alterações
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    private async Task<Result> ValidateEmailAsync(Cliente cliente, string newEmailValue, CancellationToken cancellationToken)
    {
        var emailCreateResult = Email.Create(newEmailValue);
        if (emailCreateResult.IsFailure) return Result.Failure(emailCreateResult.Error);

        var newEmail = emailCreateResult.Value;

        // Valida apenas se o email foi alterado
        if (cliente.Email.Value != newEmail.Value)
        {
            var exists = await context.Clientes.AnyAsync(c => c.Email == newEmail, cancellationToken);
            if (exists)
            {
                return Result.Failure(ClienteErrors.DuplicateEmail);
            }
        }

        return Result.Success();
    }

    private async Task<Result> ValidateDocumentoAsync(Cliente cliente, string newDocumentoValue, CancellationToken cancellationToken)
    {
        var docResult = DocumentoValidator.ValidarEIdentificarTipo(newDocumentoValue);
        if (docResult.IsFailure) return Result.Failure(docResult.Error);

        var (_, newCpf, newCnpj) = docResult.Value;

        // Valida apenas se o documento foi alterado
        bool changed = false;
        if (newCpf is not null)
        {
            changed = cliente.Cpf.Value != newCpf.Value;
            if (changed && await context.Clientes.AnyAsync(c => c.Cpf == newCpf, cancellationToken))
            {
                return Result.Failure(ClienteErrors.DuplicateDocument);
            }
        }
        else if (newCnpj is not null)
        {
            changed = cliente.Cnpj != newCnpj;
            if (changed && await context.Clientes.AnyAsync(c => c.Cnpj == newCnpj, cancellationToken))
            {
                return Result.Failure(ClienteErrors.DuplicateDocument);
            }
        }

        return Result.Success();
    }

    private async Task<Result> ValidateCodigoClienteAsync(Cliente cliente, string newCodigoCliente, CancellationToken cancellationToken)
    {
        // Valida apenas se o código foi alterado
        if (cliente.CodigoCliente != newCodigoCliente)
        {
            var exists = await context.Clientes.AnyAsync(c => c.CodigoCliente == newCodigoCliente, cancellationToken);
            if (exists)
            {
                return Result.Failure(ClienteErrors.DuplicateCodigoCliente);
            }
        }

        return Result.Success();
    }

    private async Task<Result> ValidateEnderecosAsync(Cliente cliente, List<UpdateEnderecoCommand> enderecosCommand, CancellationToken cancellationToken)
    {
        // Verificar duplicidade de CodigoInstalacao na própria lista do comando
        var codigosNoComando = enderecosCommand.Select(e => e.CodigoInstalacao).ToList();
        if (codigosNoComando.Distinct().Count() != codigosNoComando.Count)
        {
            return Result.Failure(EnderecoErrors.DuplicateCodigoInstalacao);
        }

        foreach (var enderecoCmd in enderecosCommand)
        {
            bool checkUniqueness = false;

            if (enderecoCmd.Id.HasValue)
            {
                // Edição: checar se mudou
                var enderecoExistente = cliente.Enderecos.FirstOrDefault(e => e.Id == enderecoCmd.Id.Value);
                if (enderecoExistente != null && enderecoExistente.CodigoInstalacao != enderecoCmd.CodigoInstalacao)
                {
                    checkUniqueness = true;
                }
            }
            else
            {
                checkUniqueness = true;
            }

            if (checkUniqueness)
            {
                var exists = await context.Enderecos.AnyAsync(e => e.CodigoInstalacao == enderecoCmd.CodigoInstalacao, cancellationToken);
                if (exists)
                {
                    return Result.Failure(EnderecoErrors.DuplicateCodigoInstalacao);
                }
            }
        }

        return Result.Success();
    }

    /// <summary>
    /// Processa a lista de endereços: remove os excluídos, atualiza os existentes e cria novos.
    /// </summary>
    private async Task ProcessarEnderecosAsync(
        List<UpdateEnderecoCommand> enderecosCommand,
        Cliente cliente,
        CancellationToken cancellationToken)
    {
        // Identificar endereços a serem removidos
        var commandEnderecoIds = enderecosCommand
            .Where(e => e.Id.HasValue)
            .Select(e => e.Id!.Value)
            .ToList();

        var enderecosToRemove = cliente.Enderecos
            .Where(e => !commandEnderecoIds.Contains(e.Id))
            .ToList();

        // Remover endereços e seus contratos
        foreach (var enderecoToRemove in enderecosToRemove)
        {
            if (enderecoToRemove.Contrato is not null)
            {
                context.Contratos.Remove(enderecoToRemove.Contrato);
            }
            context.Enderecos.Remove(enderecoToRemove);
        }

        // Processar cada endereço do comando
        foreach (var enderecoCmd in enderecosCommand)
        {
            // Resolver ou criar Cidade/Estado
            var cidade = await cidadeResolverService.ResolverOuCriarCidadeAsync(
                enderecoCmd.Cidade,
                enderecoCmd.Uf,
                cancellationToken);

            if (enderecoCmd.Id.HasValue)
            {
                // Atualizar endereço existente
                await AtualizarEnderecoExistenteAsync(enderecoCmd, cliente, cidade);
            }
            else
            {
                // Criar novo endereço
                await CriarNovoEnderecoAsync(enderecoCmd, cliente.Id, cidade);
            }
        }
    }

    /// <summary>
    /// Atualiza um endereço existente e seu contrato.
    /// </summary>
    private async Task AtualizarEnderecoExistenteAsync(
        UpdateEnderecoCommand enderecoCmd,
        Cliente cliente,
        Domain.Cidades.Cidade cidade)
    {
        var enderecoExistente = cliente.Enderecos.FirstOrDefault(e => e.Id == enderecoCmd.Id!.Value);
        if (enderecoExistente is null)
        {
            return;
        }

        Guid? contratoId = enderecoExistente.ContratoId;

        // Processar contrato
        if (enderecoCmd.Contrato is not null)
        {
            if (enderecoExistente.Contrato is not null)
            {
                // Atualizar contrato existente
                enderecoExistente.Contrato.ValorKwh = enderecoCmd.Contrato.ValorKwh;
                enderecoExistente.Contrato.QuantidadeKwh = enderecoCmd.Contrato.QuantidadeKwh;
                enderecoExistente.Contrato.DataInicio = enderecoCmd.Contrato.DataInicio;
                enderecoExistente.Contrato.Anexo = enderecoCmd.Contrato.Anexo;
            }
            else
            {
                // Criar novo contrato
                var novoContrato = new Contrato
                {
                    Id = Guid.NewGuid(),
                    ValorKwh = enderecoCmd.Contrato.ValorKwh,
                    QuantidadeKwh = enderecoCmd.Contrato.QuantidadeKwh,
                    DataInicio = enderecoCmd.Contrato.DataInicio,
                    Anexo = enderecoCmd.Contrato.Anexo
                };
                context.Contratos.Add(novoContrato);
                contratoId = novoContrato.Id;
            }
        }
        else
        {
            // Remover contrato se não foi fornecido
            if (enderecoExistente.Contrato is not null)
            {
                context.Contratos.Remove(enderecoExistente.Contrato);
                contratoId = null;
            }
        }

        // Atualizar endereço
        enderecoExistente.Update(
            enderecoCmd.CodigoInstalacao,
            enderecoCmd.Logradouro,
            (TipoLigacao)enderecoCmd.TipoLigacao,
            enderecoCmd.Cep,
            enderecoCmd.Numero,
            enderecoCmd.Bairro,
            cidade.Id,
            contratoId
        );

        await Task.CompletedTask;
    }

    /// <summary>
    /// Cria um novo endereço com seu contrato (se fornecido).
    /// </summary>
    private async Task CriarNovoEnderecoAsync(
        UpdateEnderecoCommand enderecoCmd,
        Guid clienteId,
        Domain.Cidades.Cidade cidade)
    {
        Guid? contratoId = null;

        // Criar contrato se fornecido
        if (enderecoCmd.Contrato is not null)
        {
            var contrato = new Contrato
            {
                Id = Guid.NewGuid(),
                ValorKwh = enderecoCmd.Contrato.ValorKwh,
                QuantidadeKwh = enderecoCmd.Contrato.QuantidadeKwh,
                DataInicio = enderecoCmd.Contrato.DataInicio,
                Anexo = enderecoCmd.Contrato.Anexo
            };
            context.Contratos.Add(contrato);
            contratoId = contrato.Id;
        }

        // Criar endereço
        var novoEndereco = Endereco.Create(
            enderecoCmd.CodigoInstalacao,
            enderecoCmd.Logradouro,
            (TipoLigacao)enderecoCmd.TipoLigacao,
            enderecoCmd.Cep,
            enderecoCmd.Numero,
            enderecoCmd.Bairro,
            cidade.Id,
            clienteId,
            contratoId
        );

        context.Enderecos.Add(novoEndereco);

        await Task.CompletedTask;
    }
}
