using ProductsAndPricingNew.Application.Common.Models;

namespace ProductsAndPricingNew.Persistence.Queries.Configuration;

internal abstract class BaseQuery
{
    protected static AddressDto? MapAddress(string? street, string? district, string? city, string? postalCode, int? countryId)
    {
        bool isEmpty = street is null && district is null && city is null && postalCode is null && countryId is null;
        return isEmpty ? null : new AddressDto(street, district, city, postalCode, countryId);
    }

    protected static ImageFileDto? MapImage(byte[]? data, string? contentType, string? fileName)
    {
        bool isEmpty = data is null && contentType is null && fileName is null;
        return isEmpty ? null : new ImageFileDto(data, contentType, fileName);
    }

    protected static string BuildEditorName(string firstName, string lastName)
        => string.Join(" ", new[] { firstName, lastName }.Where(v => !string.IsNullOrWhiteSpace(v)));

    protected static DateOnly ToDateOnly(DateTimeOffset value) =>
        DateOnly.FromDateTime(value.UtcDateTime);

    protected static string ToBase64Version(byte[]? version) =>
        Convert.ToBase64String(version ?? []);
}