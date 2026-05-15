using ProductsAndPricingNew.Domain.Common.Primitives;
using ProductsAndPricingNew.Domain.Entities.PricingRef;
using ProductsAndPricingNew.Domain.ReferenceData;
using ProductsAndPricingNew.Domain.SharedKernel.Definitions;
using ProductsAndPricingNew.Domain.SharedKernel.TextContent;

namespace ProductsAndPricingNew.UnitTests.TestSupport.Builders;

internal sealed class DivisionBuilder
{
    private int? _id;
    private byte[] _version = [1, 2, 3, 4, 5, 6, 7, 8];
    private string _name = "Division";
    private string _websiteUrl = "https://example.com";
    private bool _isActive = true;
    private int? _countryId = 1;
    private string? _street = "Street";
    private string? _district;
    private string? _city = "City";
    private string? _postalCode = "10001";
    private byte[]? _bannerData = [1, 2, 3];
    private string? _bannerContentType = "image/png";
    private string? _bannerFileName = "banner.png";

    private IReadOnlyCollection<TextContentDefinition> _texts =
    [
        new(100, null, "Text", ContentFormat.PlainText)
    ];

    public DivisionBuilder WithId(int id)
    {
        _id = id;
        return this;
    }

    public DivisionBuilder WithVersion(byte[] version)
    {
        _version = version;
        return this;
    }

    public DivisionBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public DivisionBuilder WithWebsiteUrl(string websiteUrl)
    {
        _websiteUrl = websiteUrl;
        return this;
    }

    public DivisionBuilder WithContactAddress(int? countryId, string? street = "Street", string? district = null, string? city = "City", string? postalCode = "10001")
    {
        _countryId = countryId;
        _street = street;
        _district = district;
        _city = city;
        _postalCode = postalCode;
        return this;
    }

    public DivisionBuilder WithAccreditationBanner(byte[]? data, string? contentType = "image/png", string? fileName = "banner.png")
    {
        _bannerData = data;
        _bannerContentType = contentType;
        _bannerFileName = fileName;
        return this;
    }

    public DivisionBuilder WithTexts(params TextContentDefinition[] texts)
    {
        _texts = texts;
        return this;
    }

    public Division Build()
    {
        Division division = new Division.Builder(_name, _websiteUrl)
            .IsActive(_isActive)
            .ContactAddress(new AddressDefinition(_countryId, _street, _district, _city, _postalCode))
            .AccreditationBanner(new ImageFileDefinition(_bannerData, _bannerContentType, _bannerFileName))
            .Texts(_texts)
            .Build();

        if (_id.HasValue)
            SetId(division, _id.Value);

        SetVersion(division, _version);

        return division;
    }

    internal static void SetId(Division division, int id)
    {
        typeof(Entity<int>)
            .GetProperty(nameof(Entity<int>.Id))!
            .SetValue(division, id);
    }

    internal static void SetVersion(Division division, byte[] version)
    {
        typeof(AggregateRoot<int>)
            .GetProperty(nameof(AggregateRoot<int>.Version))!
            .SetValue(division, version);
    }
}