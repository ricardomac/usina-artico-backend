using Microsoft.EntityFrameworkCore;
using UsinaArtico.Application.Abstractions.Data;
using UsinaArtico.Application.Abstractions.Messaging;
using UsinaArtico.Domain.Clientes;
using UsinaArtico.SharedKernel;

namespace UsinaArtico.Application.Clientes.ChangeStatus;

internal sealed class ChangeClienteStatusCommandHandler(IApplicationDbContext context)
    : ICommandHandler<ChangeClienteStatusCommand>
{
    public async Task<Result> Handle(ChangeClienteStatusCommand command, CancellationToken cancellationToken)
    {
        Cliente? cliente = await context.Clientes
            .FirstOrDefaultAsync(c => c.Id == command.ClienteId, cancellationToken);

        if (cliente is null)
        {
            return Result.Failure(ClienteErrors.NotFound(command.ClienteId));
        }

        cliente.SetStatus(command.IsActive);
        
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
