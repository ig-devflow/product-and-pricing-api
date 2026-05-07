using FluentValidation;

namespace ProductsAndPricingNew.Application.Common.Validation;

internal static class FluentValidationExtensions
{
    public static IRuleBuilderOptions<T, string?> IsValidEmail<T>(this IRuleBuilder<T, string?> rule)
    {
        return rule.Must(value =>
            {
                if (string.IsNullOrWhiteSpace(value))
                    return true;

                return ValidationDefaults.EmailRegex.IsMatch(value.Trim());
            })
            .WithMessage("Invalid email.");
    }

    public static IRuleBuilderOptions<T, string?> IsValidPhone<T>(this IRuleBuilder<T, string?> rule)
    {
        return rule.Must(value =>
            {
                if (string.IsNullOrWhiteSpace(value))
                    return true;

                string formatted = value.Trim();

                if (!ValidationDefaults.PhoneRegex.IsMatch(formatted))
                    return false;

                string digits = new(formatted.Where(char.IsDigit).ToArray());

                return digits.Length is >= 8 and <= 20;
            })
            .WithMessage("Phone must include area/country code and contain 8-20 digits.");
    }

    public static IRuleBuilderOptions<T, string> IsValidHttpUrl<T>(this IRuleBuilder<T, string> rule)
    {
        return rule.Must(url =>
            {
                if (string.IsNullOrWhiteSpace(url))
                    return false;

                return ValidationDefaults.HttpUrlRegex.IsMatch(url.Trim());
            })
            .WithMessage("Enter a valid website address.");
    }
}