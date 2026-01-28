namespace UsinaArtico.SharedKernel;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}
