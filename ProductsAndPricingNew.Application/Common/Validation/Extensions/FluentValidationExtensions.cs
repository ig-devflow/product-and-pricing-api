using FluentValidation;

namespace ProductsAndPricingNew.Application.Common.Validation.Extensions;

internal static class FluentValidationExtensions
{
    public static IRuleBuilderOptions<T, string?> IsValidRowVersion<T>(this IRuleBuilder<T, string?> rule)
    {
        return rule.Must(value =>
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;

            try
            {
                byte[] bytes = Convert.FromBase64String(value);
                return bytes.Length == 8;
            }
            catch (FormatException)
            {
                return false;
            }
        });
    }
}