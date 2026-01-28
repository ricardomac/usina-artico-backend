using UsinaArtico.SharedKernel;

namespace UsinaArtico.Domain.Todos;

public sealed record TodoItemCreatedDomainEvent(Guid TodoItemId) : IDomainEvent;
