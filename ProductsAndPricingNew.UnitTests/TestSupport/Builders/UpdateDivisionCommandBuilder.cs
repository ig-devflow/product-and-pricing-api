using ProductsAndPricingNew.Application.Common.Models;
using ProductsAndPricingNew.Application.Features.Division.Commands.UpdateDivision;

namespace ProductsAndPricingNew.UnitTests.TestSupport.Builders;

internal sealed class UpdateDivisionCommandBuilder
{
    private int _id = 1;
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

    public UpdateDivisionCommandBuilder WithId(int id)
    {
        _id = id;
        return this;
    }

    public UpdateDivisionCommandBuilder WithName(string? name)
    {
        _name = name;
        return this;
    }

    public UpdateDivisionCommandBuilder WithWebsiteUrl(string? websiteUrl)
    {
        _websiteUrl = websiteUrl;
        return this;
    }

    public UpdateDivisionCommandBuilder WithHeadOfficeEmail(string? email)
    {
        _headOfficeEmail = email;
        return this;
    }

    public UpdateDivisionCommandBuilder WithHeadOfficeTelephoneNo(string? phone)
    {
        _headOfficeTelephoneNo = phone;
        return this;
    }

    public UpdateDivisionCommandBuilder WithContactAddress(AddressDto? address)
    {
        _contactAddress = address;
        return this;
    }

    public UpdateDivisionCommandBuilder WithAccreditationBanner(ImageBannerDto? banner)
    {
        _accreditationBanner = banner;
        return this;
    }

    public UpdateDivisionCommandBuilder WithTexts(params TextContentDto[] texts)
    {
        _texts = texts;
        return this;
    }

    public UpdateDivisionCommandBuilder WithTermsAndConditions(string? value)
    {
        _termsAndConditions = value;
        return this;
    }

    public UpdateDivisionCommandBuilder WithGroupsPaymentTerms(string? value)
    {
        _groupsPaymentTerms = value;
        return this;
    }

    public UpdateDivisionCommand Build()
    {
        return new UpdateDivisionCommand(
            _id,
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
