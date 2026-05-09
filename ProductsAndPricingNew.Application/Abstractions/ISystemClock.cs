namespace ProductsAndPricingNew.Application.Abstractions;

public interface ISystemClock
{
    DateTimeOffset UtcNow { get; }
}