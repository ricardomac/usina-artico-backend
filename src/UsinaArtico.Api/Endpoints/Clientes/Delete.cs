using UsinaArtico.Api.Extensions;
using UsinaArtico.Api.Infrastructure;
using UsinaArtico.Application.Abstractions.Messaging;
using UsinaArtico.Application.Clientes.Delete;
using UsinaArtico.SharedKernel;
using UsinaArtico.SharedKernel.Authorization;

namespace UsinaArtico.Api.Endpoints.Clientes;

public sealed class Delete : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("api/clientes/{id}", async (
            Guid id,
            ICommandHandler<DeleteClienteCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new DeleteClienteCommand(id);

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .HasPermission(Permissions.ClientesDelete)
        .WithTags(Tags.Clientes)
        .WithSummary("Remove um cliente")
        .WithDescription("Remove um cliente existente e todos os seus dados associados.")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError);
    }
}
