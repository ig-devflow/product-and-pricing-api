namespace ProductsAndPricingNew.Domain.Utils;

public static class StringNormalizer
{
    public static string? TrimToNull(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();

    public static string TrimToEmpty(string? value) =>
        string.IsNullOrWhiteSpace(value) ? string.Empty : value.Trim();

    public static string RequireTrimmed(string? value, string fieldName)
    {
        string? normalized = TrimToNull(value);
        return normalized ?? throw new DomainException($"{fieldName} is required");
    }
}