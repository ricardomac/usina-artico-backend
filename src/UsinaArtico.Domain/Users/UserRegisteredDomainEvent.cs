using UsinaArtico.SharedKernel;

namespace UsinaArtico.Domain.Users;

public sealed record UserRegisteredDomainEvent(Guid UserId) : IDomainEvent;
