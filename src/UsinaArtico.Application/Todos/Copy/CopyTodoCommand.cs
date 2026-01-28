using UsinaArtico.Application.Abstractions.Messaging;

namespace UsinaArtico.Application.Todos.Copy;

public sealed class CopyTodoCommand : ICommand<Guid>
{
    public Guid UserId { get; set; }
    public Guid TodoId { get; set; }
}
