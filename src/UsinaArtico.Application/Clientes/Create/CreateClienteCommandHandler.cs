using Microsoft.EntityFrameworkCore;
using UsinaArtico.Application.Abstractions.Data;
using UsinaArtico.Application.Abstractions.Messaging;
using UsinaArtico.Domain.Clientes;
using UsinaArtico.SharedKernel;

namespace UsinaArtico.Application.Clientes.Create;

internal sealed class CreateClienteCommandHandler(
    IApplicationDbContext context)
    : ICommandHandler<CreateClienteCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateClienteCommand command, CancellationToken cancellationToken)
    {
        // 1. Validar e Criar Email VO
        var emailResult = Email.Create(command.Email);
        if (emailResult.IsFailure)
        {
            return Result.Failure<Guid>(emailResult.Error);
        }
        var email = emailResult.Value;

        // 2. Verificar duplicidade de Email
        if (await context.Clientes.AnyAsync(c => c.Email == email, cancellationToken))
        {
            return Result.Failure<Guid>(ClienteErrors.DuplicateEmail);
        }

        // 3. Validar Tipo de Pessoa
        if (command.Tipo != TipoPessoa.Fisica && command.Tipo != TipoPessoa.Juridica)
        {
            return Result.Failure<Guid>(Error.Failure(
                "Cliente.TipoInvalido", 
                "Tipo de pessoa deve ser Física (1) ou Jurídica (2)."));
        }

        // 4. Criar e Validar Documento (CPF ou CNPJ) baseado no Tipo
        Cpf? cpf = null;
        Cnpj? cnpj = null;

        switch (command.Tipo)
        {
            case TipoPessoa.Fisica:
                var cpfResult = Cpf.Create(command.Documento);
                if (cpfResult.IsFailure)
                {
                    return Result.Failure<Guid>(cpfResult.Error);
                }
                cpf = cpfResult.Value;

                if (await context.Clientes.AnyAsync(c => c.Cpf == cpf, cancellationToken))
                {
                    return Result.Failure<Guid>(ClienteErrors.DuplicateCpfCnpj);
                }
                break;

            case TipoPessoa.Juridica:
                var cnpjResult = Cnpj.Create(command.Documento);
                if (cnpjResult.IsFailure)
                {
                    return Result.Failure<Guid>(cnpjResult.Error);
                }
                cnpj = cnpjResult.Value;

                if (await context.Clientes.AnyAsync(c => c.Cnpj == cnpj, cancellationToken))
                {
                    return Result.Failure<Guid>(ClienteErrors.DuplicateCpfCnpj);
                }
                break;

            default:
                return Result.Failure<Guid>(Error.Failure(
                    "Cliente.TipoInvalido", 
                    "Tipo de pessoa deve ser Física (1) ou Jurídica (2)."));
        }

        // 7. Criar Entidade usando Factory Method
        var cliente = Cliente.Create(
            command.Nome,
            email,
            command.Telefone,
            command.CodigoCliente,
            command.Tipo,
            cpf,
            cnpj
        );

        // 8. Persistir
        context.Clientes.Add(cliente);
        await context.SaveChangesAsync(cancellationToken);

        return cliente.Id;
    }
}
