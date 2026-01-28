using UsinaArtico.Api.Extensions;
using UsinaArtico.Api.Infrastructure;
using UsinaArtico.Application.Abstractions.Messaging;
using UsinaArtico.Application.Todos.Complete;
using UsinaArtico.SharedKernel;

namespace UsinaArtico.Api.Endpoints.Todos;

internal sealed class Complete : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("todos/{id:guid}/complete", async (
            Guid id,
            ICommandHandler<CompleteTodoCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new CompleteTodoCommand(id);

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(Tags.Todos)
        .RequireAuthorization();
    }
}
