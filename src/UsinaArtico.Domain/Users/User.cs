using Microsoft.AspNetCore.Identity;
using UsinaArtico.Domain.Enums;
using UsinaArtico.SharedKernel;

namespace UsinaArtico.Domain.Users;

public sealed class User : IdentityUser<Guid>, IHasDomainEvents
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;

    private readonly List<IDomainEvent> _domainEvents = [];

    public List<IDomainEvent> DomainEvents => [.. _domainEvents];

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    public void Raise(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
}
