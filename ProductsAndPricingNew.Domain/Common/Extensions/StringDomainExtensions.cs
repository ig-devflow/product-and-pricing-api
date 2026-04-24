using System.Text;
using System.Text.RegularExpressions;
using ProductsAndPricingNew.Domain.Common.Errors;

namespace ProductsAndPricingNew.Domain.Common.Extensions;

public static class StringDomainExtensions
{
    private static readonly Regex WhitespaceRegex = new(@"\s+", RegexOptions.Compiled, TimeSpan.FromSeconds(1));

    public static string? AsOptionalDomainText(this string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;

        return Normalize(value);
    }

    public static string AsRequiredDomainText(this string? value)
    {
        string? normalized = value.AsOptionalDomainText();

        if (normalized is null)
            throw new DomainException("Required text cannot be empty.");

        return normalized;
    }

    private static string Normalize(string value)
    {
        string normalized = value
            .Normalize(NormalizationForm.FormKC)
            .Replace('\u00A0', ' ')
            .Trim();

        return WhitespaceRegex.Replace(normalized, " ");
    }
}