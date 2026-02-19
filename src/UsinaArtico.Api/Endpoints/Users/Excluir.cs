using UsinaArtico.Api.Endpoints;
using UsinaArtico.Api.Extensions;
using UsinaArtico.Api.Infrastructure;
using UsinaArtico.Application.Abstractions.Messaging;
using UsinaArtico.Application.Users.Excluir;
using UsinaArtico.SharedKernel;
using UsinaArtico.SharedKernel.Authorization;

namespace UsinaArtico.Api.Endpoints.Users;

public sealed class Excluir : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/users/{userId}", async (
            Guid userId,
            ICommandHandler<ExcluirUsuarioCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new ExcluirUsuarioCommand(userId);

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(() => Results.NoContent(), CustomResults.Problem);
        })
        .HasPermission(Permissions.UsuariosDelete)
        .WithTags(Tags.Users);
    }
}
