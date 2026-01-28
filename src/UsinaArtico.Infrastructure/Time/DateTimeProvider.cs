using UsinaArtico.SharedKernel;

namespace UsinaArtico.Infrastructure.Time;

internal sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}
