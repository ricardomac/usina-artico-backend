using Microsoft.EntityFrameworkCore;
using UsinaArtico.Application.Abstractions.Authentication;
using UsinaArtico.Application.Abstractions.Data;
using UsinaArtico.Application.Abstractions.Messaging;
using UsinaArtico.Domain.Users;
using UsinaArtico.SharedKernel;

namespace UsinaArtico.Application.Todos.Get;

internal sealed class GetTodosQueryHandler(IApplicationDbContext context, IUserContext userContext)
    : IQueryHandler<GetTodosQuery, List<TodoResponse>>
{
    public async Task<Result<List<TodoResponse>>> Handle(GetTodosQuery query, CancellationToken cancellationToken)
    {
        if (query.UserId != userContext.UserId)
        {
            return Result.Failure<List<TodoResponse>>(UserErrors.Unauthorized());
        }

        List<TodoResponse> todos = await context.TodoItems
            .Where(todoItem => todoItem.UserId == query.UserId)
            .Select(todoItem => new TodoResponse
            {
                Id = todoItem.Id,
                UserId = todoItem.UserId,
                Description = todoItem.Description,
                DueDate = todoItem.DueDate,
                Labels = todoItem.Labels,
                IsCompleted = todoItem.IsCompleted,
                CreatedAt = todoItem.CreatedAt,
                CompletedAt = todoItem.CompletedAt
            })
            .ToListAsync(cancellationToken);

        return todos;
    }
}
