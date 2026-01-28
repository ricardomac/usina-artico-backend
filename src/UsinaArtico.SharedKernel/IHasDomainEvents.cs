namespace UsinaArtico.SharedKernel;

public interface IHasDomainEvents
{
    List<IDomainEvent> DomainEvents { get; }
    void ClearDomainEvents();
}
