using ProductsAndPricingNew.Application.Common.Models;
using ProductsAndPricingNew.Application.Features.Division.Commands.CreateDivision;

namespace ProductsAndPricingNew.UnitTests.TestSupport.Builders;

internal sealed class CreateDivisionCommandBuilder
{
    private string? _name = "Division";
    private string? _websiteUrl = "https://example.com";
    private bool _isActive = true;
    private string? _termsAndConditions;
    private string? _groupsPaymentTerms;
    private string? _headOfficeEmail = "office@example.com";
    private string? _headOfficeTelephoneNo = "+1 555 123 4567";
    private AddressDto? _contactAddress = new AddressDtoBuilder().Build();
    private ImageBannerDto? _accreditationBanner = new ImageBannerDtoBuilder().Build();
    private IReadOnlyCollection<TextContentDto>? _texts = [new TextContentDtoBuilder().Build()];

    public CreateDivisionCommandBuilder WithName(string? name)
    {
        _name = name;
        return this;
    }

    public CreateDivisionCommandBuilder WithWebsiteUrl(string? websiteUrl)
    {
        _websiteUrl = websiteUrl;
        return this;
    }

    public CreateDivisionCommandBuilder WithHeadOfficeEmail(string? email)
    {
        _headOfficeEmail = email;
        return this;
    }

    public CreateDivisionCommandBuilder WithHeadOfficeTelephoneNo(string? phone)
    {
        _headOfficeTelephoneNo = phone;
        return this;
    }

    public CreateDivisionCommandBuilder WithContactAddress(AddressDto? address)
    {
        _contactAddress = address;
        return this;
    }

    public CreateDivisionCommandBuilder WithAccreditationBanner(ImageBannerDto? banner)
    {
        _accreditationBanner = banner;
        return this;
    }

    public CreateDivisionCommandBuilder WithTexts(params TextContentDto[] texts)
    {
        _texts = texts;
        return this;
    }

    public CreateDivisionCommandBuilder WithTermsAndConditions(string? value)
    {
        _termsAndConditions = value;
        return this;
    }

    public CreateDivisionCommandBuilder WithGroupsPaymentTerms(string? value)
    {
        _groupsPaymentTerms = value;
        return this;
    }

    public CreateDivisionCommand Build()
    {
        return new CreateDivisionCommand(
            _name!,
            _websiteUrl!,
            _isActive,
            _termsAndConditions,
            _groupsPaymentTerms,
            _headOfficeEmail,
            _headOfficeTelephoneNo,
            _contactAddress,
            _accreditationBanner,
            _texts!);
    }
}
