using Microsoft.EntityFrameworkCore;
using UsinaArtico.Application.Abstractions.Data;
using UsinaArtico.Application.Abstractions.Messaging;
using UsinaArtico.Domain.Clientes;
using UsinaArtico.SharedKernel;

namespace UsinaArtico.Application.Clientes.Delete;

internal sealed class DeleteClienteCommandHandler(IApplicationDbContext context)
    : ICommandHandler<DeleteClienteCommand>
{
    public async Task<Result> Handle(DeleteClienteCommand command, CancellationToken cancellationToken)
    {
        var cliente = await context.Clientes
            .SingleOrDefaultAsync(c => c.Id == command.ClienteId, cancellationToken);

        if (cliente is null)
        {
            return Result.Failure(ClienteErrors.NotFound(command.ClienteId));
        }

        context.Clientes.Remove(cliente);
        
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
