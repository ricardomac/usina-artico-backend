using UsinaArtico.Api.Extensions;
using UsinaArtico.Api.Infrastructure;
using UsinaArtico.Application.Abstractions.Messaging;
using UsinaArtico.Application.Users.Update;
using UsinaArtico.SharedKernel;
using UsinaArtico.SharedKernel.Authorization;

namespace UsinaArtico.Api.Endpoints.Users;

public sealed class Update : IEndpoint
{
    public sealed record Request(string Email, string FirstName, string LastName, string? Password, string RoleName);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/users/{userId}", async (
            Guid userId,
            Request request,
            ICommandHandler<UpdateUserCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new UpdateUserCommand(
                userId,
                request.Email,
                request.FirstName,
                request.LastName,
                request.Password,
                request.RoleName);

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .HasPermission(Permissions.UsuariosUpdate)
        .WithTags(Tags.Users);
    }
}