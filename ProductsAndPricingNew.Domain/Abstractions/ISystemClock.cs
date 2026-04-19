namespace ProductsAndPricingNew.Domain.Abstractions;

public interface ISystemClock
{
    DateTimeOffset UtcNow { get; }
}