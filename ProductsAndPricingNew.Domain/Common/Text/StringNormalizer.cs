using System.Text;
using System.Text.RegularExpressions;
using ProductsAndPricingNew.Domain.Common.Exceptions;

namespace ProductsAndPricingNew.Domain.Common.Text;

public static class StringDomainExtensions
{
    private static readonly Regex WhitespaceRegex = new(@"\s+", RegexOptions.Compiled | RegexOptions.CultureInvariant, TimeSpan.FromSeconds(1));

    public static string? AsOptionalText(this string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;

        return Normalize(value);
    }

    public static string AsRequiredText(this string? value)
    {
        string? normalized = value.AsOptionalText();

        if (normalized is null)
            throw new ArgumentNullException($"Text is required.");

        return normalized;
    }

    internal static string? AsOptionalDomainText(this string? value, string fieldName, int? maxLength = null)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;

        string normalized = Normalize(value);
        normalized.EnsureTextMaxLength(fieldName, maxLength);

        return normalized;
    }

    internal static string AsRequiredDomainText(this string? value, string fieldName, int? maxLength = null)
    {
        string? normalized = value.AsOptionalDomainText(fieldName, maxLength);

        if (normalized is null)
            throw new DomainException($"{fieldName} is required.");

        return normalized;
    }

    private static void EnsureTextMaxLength(this string? value, string fieldName, int? maxLength)
    {
        if (maxLength.HasValue && maxLength <= 0)
            throw new ArgumentOutOfRangeException(nameof(maxLength), "Max length must be greater than zero.");

        if (value is not null && maxLength.HasValue && value.Length > maxLength)
            throw new DomainException($"{fieldName} must not exceed {maxLength} characters.");
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