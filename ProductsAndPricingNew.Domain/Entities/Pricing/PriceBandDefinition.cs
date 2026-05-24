namespace ProductsAndPricingNew.Domain.Entities.Pricing;

public sealed record PriceBandDefinition(int MinUnits, int MaxUnits, decimal PricePerUnit);