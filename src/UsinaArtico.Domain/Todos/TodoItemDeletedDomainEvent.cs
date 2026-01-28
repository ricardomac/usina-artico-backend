using UsinaArtico.SharedKernel;

namespace UsinaArtico.Domain.Todos;

public sealed record TodoItemDeletedDomainEvent(Guid TodoItemId) : IDomainEvent;
