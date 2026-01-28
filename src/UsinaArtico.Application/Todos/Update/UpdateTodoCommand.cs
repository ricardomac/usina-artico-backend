using UsinaArtico.Application.Abstractions.Messaging;

namespace UsinaArtico.Application.Todos.Update;

public sealed record UpdateTodoCommand(
    Guid TodoItemId,
    string Description) : ICommand;
