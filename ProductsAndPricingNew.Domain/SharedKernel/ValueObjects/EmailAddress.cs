using System.Text.RegularExpressions;
using ProductsAndPricingNew.Domain.Common.Exceptions;
using ProductsAndPricingNew.Domain.Common.Text;

namespace ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;

public readonly record struct EmailAddress : IEmptyValueObject
{
    private static readonly Regex EmailRegex = new(@"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase, TimeSpan.FromSeconds(1));

    public string? Value { get; }

    private EmailAddress(string? value)
    {
        Value = value;
    }

    public bool IsEmpty => Value is null;
    public static EmailAddress Empty { get; } = new(null);

    public static EmailAddress Create(string? value)
    {
        string? normalized = value.AsOptionalDomainText(nameof(EmailAddress), Rules.MaxLength);

        if (normalized is null)
            return Empty;

        EnsureValid(normalized);

        return new EmailAddress(normalized);
    }

    public static bool IsValid(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return true;

        string normalized = value.AsOptionalText()!;

        return normalized.Length <= Rules.MaxLength && EmailRegex.IsMatch(normalized);
    }

    private static void EnsureValid(string value)
    {
        if (!EmailRegex.IsMatch(value))
            throw new DomainException("Email must be a valid email address.");
    }

    public static class Rules
    {
        public const int MaxLength = 100;
    }
}