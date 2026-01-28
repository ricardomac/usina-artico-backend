using UsinaArtico.Api.Extensions;
using UsinaArtico.Api.Infrastructure;
using UsinaArtico.Application.Abstractions.Messaging;
using UsinaArtico.Application.Roles.Create;
using UsinaArtico.SharedKernel;

namespace UsinaArtico.Api.Endpoints.Roles;

public sealed class Create : IEndpoint
{
    public sealed record Request(string Name);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/roles", async (
            Request request,
            ICommandHandler<CreateRoleCommand, Guid> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new CreateRoleCommand(request.Name);

            Result<Guid> result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags("Roles")
        .RequireAuthorization(); // Add Policy later if needed, e.g. Admin only
    }
}
