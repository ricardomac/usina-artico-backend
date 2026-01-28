using UsinaArtico.Application.Abstractions.Messaging;

namespace UsinaArtico.Application.Todos.Get;

public sealed record GetTodosQuery(Guid UserId) : IQuery<List<TodoResponse>>;
