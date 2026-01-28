using UsinaArtico.SharedKernel;

namespace UsinaArtico.Domain.Todos;

public sealed record TodoItemCompletedDomainEvent(Guid TodoItemId) : IDomainEvent;
