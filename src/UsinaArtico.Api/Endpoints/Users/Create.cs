using UsinaArtico.Api.Extensions;
using UsinaArtico.Api.Infrastructure;
using UsinaArtico.Application.Abstractions.Messaging;
using UsinaArtico.Application.Users.Create;
using UsinaArtico.SharedKernel;

namespace UsinaArtico.Api.Endpoints.Users;

public sealed class Create : IEndpoint
{
    public sealed record Request(string Email, string FirstName, string LastName, string Password, string RoleName);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/users", async (
            Request request,
            ICommandHandler<CreateUserCommand, Guid> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new CreateUserCommand(
                request.Email,
                request.FirstName,
                request.LastName,
                request.Password,
               request.RoleName);

            Result<Guid> result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Users);
    }
}
