using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;

namespace ProductsAndPricingNew.Persistence.Configuration;

internal static class Converters
{
    public static readonly ValueConverter<EmailAddress, string?> EmailAddress = new(
        v => v.IsEmpty ? null : v.Value,
        v => Domain.SharedKernel.ValueObjects.EmailAddress.Create(v)
    );

    public static readonly ValueConverter<TelephoneNumber, string?> TelephoneNumber = new(
        v => v.IsEmpty ? null : v.Value,
        v => Domain.SharedKernel.ValueObjects.TelephoneNumber.Create(v)
    );

    public static readonly ValueConverter<HexColor, string?> HexColor = new(
        v => v.IsEmpty ? null : v.Value,
        v => Domain.SharedKernel.ValueObjects.HexColor.Create(v)
    );

    public static readonly ValueConverter<WebsiteUrl, string?> WebsiteUrl = new(
        v => v.IsEmpty ? null : v.Value,
        v => Domain.SharedKernel.ValueObjects.WebsiteUrl.Create(v)
    );
}