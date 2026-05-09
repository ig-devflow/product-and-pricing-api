using System.Text.RegularExpressions;

namespace ProductsAndPricingNew.Application.Common.Validation.Patterns;

internal static class ValidationPatterns
{
    public static readonly Regex EmailRegex = new(
        @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$",
        RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase,
        TimeSpan.FromSeconds(1));

    public static readonly Regex PhoneRegex = new(
        @"^[\d\s\-\(\)\+]+$",
        RegexOptions.Compiled | RegexOptions.CultureInvariant,
        TimeSpan.FromSeconds(1));

    public static readonly Regex HttpUrlRegex = new(
        @"^https?:\/\/(?:[a-z0-9](?:[a-z0-9\-]{0,61}[a-z0-9])\.)+[a-z]{2,}(?::\d{2,5})?(?:\/\S*)?$",
        RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase,
        TimeSpan.FromSeconds(1));
}
