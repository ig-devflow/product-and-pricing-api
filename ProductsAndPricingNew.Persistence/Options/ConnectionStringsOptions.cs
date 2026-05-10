using System.ComponentModel.DataAnnotations;

namespace ProductsAndPricingNew.Persistence.Options;

public sealed class ConnectionStringsOptions
{
    public const string SectionName = "ConnectionStrings";

    [Required, MinLength(1)]
    public string ProductsAndPricing { get; set; } = string.Empty;
}