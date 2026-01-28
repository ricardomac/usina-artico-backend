using UsinaArtico.Api.Extensions;
using UsinaArtico.Api.Infrastructure;
using UsinaArtico.Application.Abstractions.Messaging;
using UsinaArtico.Application.Todos.Copy;
using UsinaArtico.SharedKernel;

namespace UsinaArtico.Api.Endpoints.Todos;

internal sealed class Copy : IEndpoint
{
    public sealed class Request
    {
        public Guid UserId { get; set; }
        public Guid TodoId { get; set; }
    }

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("todos/{todoId}/copy", async (
            Guid todoId,
            Request request,
            ICommandHandler<CopyTodoCommand, Guid> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new CopyTodoCommand
            {
                UserId = request.UserId,
                TodoId = todoId
            };

            Result<Guid> result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Todos)
        .RequireAuthorization();
    }
}
