using UsinaArtico.Application.Abstractions.Messaging;

namespace UsinaArtico.Application.Todos.Complete;

public sealed record CompleteTodoCommand(Guid TodoItemId) : ICommand;
