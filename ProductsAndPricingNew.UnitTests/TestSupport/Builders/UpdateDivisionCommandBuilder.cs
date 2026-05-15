using ProductsAndPricingNew.Application.Common.Models;
using ProductsAndPricingNew.Application.Features.Division.Commands.UpdateDivision;
using ProductsAndPricingNew.Domain.ReferenceData;
using ProductsAndPricingNew.Domain.SharedKernel.TextContent;

namespace ProductsAndPricingNew.UnitTests.TestSupport.Builders;

internal sealed class UpdateDivisionCommandBuilder
{
    private int _id = 1;
    private string _version = Convert.ToBase64String([1, 2, 3, 4, 5, 6, 7, 8]);
    private string _name = "Division";
    private string _websiteUrl = "https://example.com";
    private bool _isActive = true;
    private string? _termsAndConditions = "Terms";
    private string? _groupsPaymentTerms = "Payment terms";
    private string? _headOfficeEmail = "head@example.com";
    private string? _headOfficeTelephoneNo = "+1 555 123 4567";
    private AddressDto? _contactAddress = new AddressDtoBuilder().Build();
    private ImageFileDto? _accreditationBanner = new ImageBannerDtoBuilder().Build();

    private IReadOnlyCollection<TextContentDto> _texts =
    [
        new TextContentDtoBuilder()
            .WithContentTemplateId(100)
            .WithContent("Text")
            .WithFormat(ContentFormat.PlainText)
            .Build()
    ];

    public UpdateDivisionCommandBuilder WithId(int id)
    {
        _id = id;
        return this;
    }

    public UpdateDivisionCommandBuilder WithVersion(string version)
    {
        _version = version;
        return this;
    }

    public UpdateDivisionCommandBuilder WithVersion(byte[] version)
    {
        _version = Convert.ToBase64String(version);
        return this;
    }

    public UpdateDivisionCommandBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public UpdateDivisionCommandBuilder WithWebsiteUrl(string websiteUrl)
    {
        _websiteUrl = websiteUrl;
        return this;
    }

    public UpdateDivisionCommandBuilder WithTermsAndConditions(string? termsAndConditions)
    {
        _termsAndConditions = termsAndConditions;
        return this;
    }

    public UpdateDivisionCommandBuilder WithGroupsPaymentTerms(string? groupsPaymentTerms)
    {
        _groupsPaymentTerms = groupsPaymentTerms;
        return this;
    }

    public UpdateDivisionCommandBuilder WithHeadOfficeEmail(string? headOfficeEmail)
    {
        _headOfficeEmail = headOfficeEmail;
        return this;
    }

    public UpdateDivisionCommandBuilder WithHeadOfficeTelephoneNo(string? headOfficeTelephoneNo)
    {
        _headOfficeTelephoneNo = headOfficeTelephoneNo;
        return this;
    }

    public UpdateDivisionCommandBuilder WithContactAddress(AddressDto? contactAddress)
    {
        _contactAddress = contactAddress;
        return this;
    }

    public UpdateDivisionCommandBuilder WithAccreditationBanner(ImageFileDto? accreditationBanner)
    {
        _accreditationBanner = accreditationBanner;
        return this;
    }

    public UpdateDivisionCommandBuilder WithTexts(params TextContentDto[] texts)
    {
        _texts = texts;
        return this;
    }

    public UpdateDivisionCommand Build()
        => new(
            _id,
            _name,
            _websiteUrl,
            _isActive,
            _termsAndConditions,
            _groupsPaymentTerms,
            _headOfficeEmail,
            _headOfficeTelephoneNo,
            _contactAddress,
            _accreditationBanner,
            _texts,
            _version);
}