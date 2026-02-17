using UsinaArtico.Api.Extensions;
using UsinaArtico.Api.Infrastructure;
using UsinaArtico.Application.Abstractions.Messaging;
using UsinaArtico.Application.Users.ChangeStatus;
using UsinaArtico.SharedKernel;
using UsinaArtico.SharedKernel.Authorization;

namespace UsinaArtico.Api.Endpoints.Users;

internal sealed class ChangeStatus : IEndpoint
{
    public sealed record Request(bool IsActive);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/users/{userId}/status", async (
            Guid userId,
            Request request,
            ICommandHandler<ChangeUserStatusCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new ChangeUserStatusCommand(userId, request.IsActive);

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .HasPermission(Permissions.UsuariosUpdate)
        .WithTags(Tags.Users);
    }
}
