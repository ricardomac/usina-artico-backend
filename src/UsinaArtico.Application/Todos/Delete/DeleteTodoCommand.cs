using UsinaArtico.Application.Abstractions.Messaging;

namespace UsinaArtico.Application.Todos.Delete;

public sealed record DeleteTodoCommand(Guid TodoItemId) : ICommand;
