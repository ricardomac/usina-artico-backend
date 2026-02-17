using UsinaArtico.Api.Extensions;
using UsinaArtico.Api.Infrastructure;
using UsinaArtico.Application.Abstractions.Messaging;
using UsinaArtico.Application.Clientes.ChangeStatus;
using UsinaArtico.SharedKernel;
using UsinaArtico.SharedKernel.Authorization;

namespace UsinaArtico.Api.Endpoints.Clientes;

internal sealed class ChangeStatus : IEndpoint
{
    public sealed record Request(bool IsActive);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/clientes/{clienteId}/status", async (
            Guid clienteId,
            Request request,
            ICommandHandler<ChangeClienteStatusCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new ChangeClienteStatusCommand(clienteId, request.IsActive);

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .HasPermission(Permissions.ClientesUpdate)
        .WithTags(Tags.Clientes)
        .WithSummary("Altera o status de um cliente")
        .WithDescription("Ativa ou desativa um cliente no sistema.")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status500InternalServerError);
    }
}
