using Domain.Common;

namespace Infrastructure.Time;

internal sealed class Clock : IClock
{
    public DateTime Current() => DateTime.UtcNow;
}