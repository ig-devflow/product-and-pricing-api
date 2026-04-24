using ProductsAndPricingNew.Domain.Abstractions;

namespace ProductsAndPricingNew.AdminApi.Infrastructure;

public sealed class SystemClock : ISystemClock
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}