using UsinaArtico.Application.Abstractions.Messaging;

namespace UsinaArtico.Application.Todos.GetById;

public sealed record GetTodoByIdQuery(Guid TodoItemId) : IQuery<TodoResponse>;
