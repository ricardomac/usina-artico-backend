using Microsoft.EntityFrameworkCore;
using UsinaArtico.Application.Abstractions.Data;
using UsinaArtico.Application.Abstractions.Messaging;
using UsinaArtico.Application.Abstractions.Services;
using UsinaArtico.Application.Clientes.Shared;
using UsinaArtico.Domain.Clientes;
using UsinaArtico.Domain.Contratos;
using UsinaArtico.Domain.Enderecos;
using UsinaArtico.SharedKernel;

namespace UsinaArtico.Application.Clientes.Create;

/// <summary>
/// Handler responsável por criar um novo cliente no sistema.
/// </summary>
internal sealed class CreateClienteCommandHandler(
    IApplicationDbContext context,
    ICidadeResolverService cidadeResolverService)
    : ICommandHandler<CreateClienteCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateClienteCommand command, CancellationToken cancellationToken)
    {
        // 1. Validar e criar Email
        var emailResult = Email.Create(command.Email);
        if (emailResult.IsFailure)
        {
            return Result.Failure<Guid>(emailResult.Error);
        }
        var email = emailResult.Value;

        // 2. Verificar unicidade do Email
        if (await context.Clientes.AnyAsync(c => c.Email == email, cancellationToken))
        {
            return Result.Failure<Guid>(ClienteErrors.DuplicateEmail);
        }

        // 3. Validar documento e identificar tipo de pessoa (CPF/CNPJ)
        var documentoResult = DocumentoValidator.ValidarEIdentificarTipo(command.Documento);
        if (documentoResult.IsFailure)
        {
            return Result.Failure<Guid>(documentoResult.Error);
        }

        var (tipoPessoa, cpf, cnpj) = documentoResult.Value;

        // 4. Verificar unicidade do CPF ou CNPJ
        var duplicateCheckResult = await VerificarDuplicidadeDocumentoAsync(cpf, cnpj, cancellationToken);
        if (duplicateCheckResult.IsFailure)
        {
            return Result.Failure<Guid>(duplicateCheckResult.Error);
        }

        // 5. Criar entidade Cliente
        var cliente = Cliente.Create(
            command.Nome,
            email,
            command.Telefone,
            command.CodigoCliente,
            tipoPessoa,
            cpf,
            cnpj
        );

        context.Clientes.Add(cliente);

        // 6. Processar endereços e contratos
        if (command.Enderecos is not null && command.Enderecos.Count > 0)
        {
            await ProcessarEnderecosAsync(command.Enderecos, cliente.Id, cancellationToken);
        }

        // 7. Salvar todas as alterações
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success(cliente.Id);
    }

    /// <summary>
    /// Verifica se já existe um cliente com o mesmo CPF ou CNPJ.
    /// </summary>
    private async Task<Result> VerificarDuplicidadeDocumentoAsync(
        Cpf? cpf,
        Cnpj? cnpj,
        CancellationToken cancellationToken)
    {
        if (cpf is not null)
        {
            if (await context.Clientes.AnyAsync(c => c.Cpf == cpf, cancellationToken))
            {
                return Result.Failure(ClienteErrors.DuplicateDocument);
            }
        }
        else if (cnpj is not null)
        {
            if (await context.Clientes.AnyAsync(c => c.Cnpj == cnpj, cancellationToken))
            {
                return Result.Failure(ClienteErrors.DuplicateDocument);
            }
        }

        return Result.Success();
    }

    /// <summary>
    /// Processa a lista de endereços, resolvendo cidades e criando contratos.
    /// </summary>
    private async Task ProcessarEnderecosAsync(
        List<EnderecoDto> enderecoDtos,
        Guid clienteId,
        CancellationToken cancellationToken)
    {
        foreach (var enderecoDto in enderecoDtos)
        {
            // Resolver ou criar Cidade/Estado
            var cidade = await cidadeResolverService.ResolverOuCriarCidadeAsync(
                enderecoDto.Cidade,
                enderecoDto.Uf,
                cancellationToken);

            // Criar contrato se fornecido
            Guid? contratoId = null;
            if (enderecoDto.Contrato is not null)
            {
                contratoId = await CriarContratoAsync(enderecoDto.Contrato);
            }

            // Criar endereço
            var endereco = Endereco.Create(
                enderecoDto.CodigoInstalacao,
                enderecoDto.Logradouro,
                (TipoLigacao)enderecoDto.TipoLigacao,
                enderecoDto.Cep,
                enderecoDto.Numero,
                enderecoDto.Bairro,
                cidade.Id,
                clienteId,
                contratoId
            );

            context.Enderecos.Add(endereco);
        }
    }

    /// <summary>
    /// Cria um novo contrato e retorna seu ID.
    /// </summary>
    private async Task<Guid> CriarContratoAsync(ContratoDto contratoDto)
    {
        var contrato = new Contrato
        {
            Id = Guid.NewGuid(),
            ValorKwh = contratoDto.ValorKwh,
            QuantidadeKwh = contratoDto.QuantidadeKwh,
            DataInicio = contratoDto.DataInicio,
            Anexo = contratoDto.Anexo
        };

        context.Contratos.Add(contrato);

        return await Task.FromResult(contrato.Id);
    }
}
