using FluentValidation.Results;
using ProductsAndPricingNew.Application.Features.Division.Commands.CreateDivision;
using ProductsAndPricingNew.Application.Features.Division.Commands.UpdateDivision;
using ProductsAndPricingNew.Domain.Entities.PricingRef;
using ProductsAndPricingNew.Domain.ReferenceData;
using ProductsAndPricingNew.UnitTests.TestSupport.Builders;
using ProductsAndPricingNew.UnitTests.TestSupport.Fakes;

namespace ProductsAndPricingNew.UnitTests.Application.Divisions;

public sealed class DivisionCommandValidatorTests
{
    [Fact]
    public async Task CreateDivisionCommand_ValidCommand_IsValid()
    {
        CreateDivisionCommandValidator validator = new(ValidReferenceData());

        ValidationResult result = await validator.ValidateAsync(new CreateDivisionCommandBuilder().Build());

        Assert.True(result.IsValid);
    }

    [Fact]
    public async Task UpdateDivisionCommand_ValidCommand_IsValid()
    {
        UpdateDivisionCommandValidator validator = new(ValidReferenceData());

        ValidationResult result = await validator.ValidateAsync(new UpdateDivisionCommandBuilder().Build());

        Assert.True(result.IsValid);
    }

    [Fact]
    public async Task Name_IsRequired()
    {
        ValidationResult result = await ValidateCreateAsync(new CreateDivisionCommandBuilder().WithName(" ").Build());

        AssertInvalid(result, "Division name is required.");
    }

    [Fact]
    public async Task Name_MaxLengthComesFromDivisionRules()
    {
        string tooLong = new('A', Division.Rules.NameMaxLength + 1);

        ValidationResult result = await ValidateCreateAsync(new CreateDivisionCommandBuilder().WithName(tooLong).Build());

        AssertInvalid(result, Division.Rules.NameMaxLength.ToString());
    }

    [Fact]
    public async Task WebsiteUrl_IsRequired()
    {
        ValidationResult result = await ValidateCreateAsync(new CreateDivisionCommandBuilder().WithWebsiteUrl(" ").Build());

        AssertInvalid(result, "Website is required.");
    }

    [Fact]
    public async Task WebsiteUrl_MaxLengthComesFromDivisionRules()
    {
        string tooLong = new('A', Division.Rules.WebsiteUrlMaxLength + 1);

        ValidationResult result = await ValidateCreateAsync(new CreateDivisionCommandBuilder().WithWebsiteUrl(tooLong).Build());

        AssertInvalid(result, Division.Rules.WebsiteUrlMaxLength.ToString());
    }

    [Fact]
    public async Task WebsiteUrl_MustBeValidHttpOrHttpsUrl()
    {
        ValidationResult result = await ValidateCreateAsync(new CreateDivisionCommandBuilder().WithWebsiteUrl("ftp://example.com").Build());

        AssertInvalid(result, "Enter a valid website address.");
    }

    [Fact]
    public async Task TermsAndConditions_MaxLengthComesFromDivisionRules()
    {
        string tooLong = new('A', Division.Rules.TermsAndConditionsMaxLength + 1);

        ValidationResult result = await ValidateCreateAsync(new CreateDivisionCommandBuilder().WithTermsAndConditions(tooLong).Build());

        AssertInvalid(result, Division.Rules.TermsAndConditionsMaxLength.ToString());
    }

    [Fact]
    public async Task GroupsPaymentTerms_MaxLengthComesFromDivisionRules()
    {
        string tooLong = new('A', Division.Rules.GroupsPaymentTermsMaxLength + 1);

        ValidationResult result = await ValidateCreateAsync(new CreateDivisionCommandBuilder().WithGroupsPaymentTerms(tooLong).Build());

        AssertInvalid(result, Division.Rules.GroupsPaymentTermsMaxLength.ToString());
    }

    [Fact]
    public async Task HeadOfficeEmail_MaxLengthComesFromDivisionRules()
    {
        string tooLong = new('A', Division.Rules.HeadOfficeEmailMaxLength + 1);

        ValidationResult result = await ValidateCreateAsync(new CreateDivisionCommandBuilder().WithHeadOfficeEmail(tooLong).Build());

        AssertInvalid(result, Division.Rules.HeadOfficeEmailMaxLength.ToString());
    }

    [Fact]
    public async Task HeadOfficeEmail_MustBeValidEmail()
    {
        ValidationResult result = await ValidateCreateAsync(new CreateDivisionCommandBuilder().WithHeadOfficeEmail("not-an-email").Build());

        AssertInvalid(result, "Invalid email.");
    }

    [Fact]
    public async Task HeadOfficeTelephoneNo_MaxLengthComesFromDivisionRules()
    {
        string tooLong = new('1', Division.Rules.HeadOfficeTelephoneNoMaxLength + 1);

        ValidationResult result = await ValidateCreateAsync(new CreateDivisionCommandBuilder().WithHeadOfficeTelephoneNo(tooLong).Build());

        AssertInvalid(result, Division.Rules.HeadOfficeTelephoneNoMaxLength.ToString());
    }

    [Fact]
    public async Task HeadOfficeTelephoneNo_MustBeValidPhone()
    {
        ValidationResult result = await ValidateCreateAsync(new CreateDivisionCommandBuilder().WithHeadOfficeTelephoneNo("123").Build());

        AssertInvalid(result, "Phone must include area/country code and contain 8-20 digits.");
    }

    [Fact]
    public async Task ContactAddress_WithUnknownCountryId_IsInvalid()
    {
        ValidationResult result = await ValidateCreateAsync(new CreateDivisionCommandBuilder()
            .WithContactAddress(new AddressDtoBuilder().WithCountryId(999).Build())
            .Build());

        AssertInvalid(result, "CountryId must reference an active country.");
    }

    [Fact]
    public async Task ContactAddress_WithActiveCountryId_IsValid()
    {
        ValidationResult result = await ValidateCreateAsync(new CreateDivisionCommandBuilder()
            .WithContactAddress(new AddressDtoBuilder().WithCountryId(1).Build())
            .Build());

        Assert.True(result.IsValid);
    }

    [Fact]
    public async Task AccreditationBanner_WithUnsupportedContentType_IsInvalid()
    {
        ValidationResult result = await ValidateCreateAsync(new CreateDivisionCommandBuilder()
            .WithAccreditationBanner(new ImageBannerDtoBuilder().WithContentType("text/plain").Build())
            .Build());

        AssertInvalid(result, "Content type must be PNG/JPEG/WEBP/SVG.");
    }

    [Fact]
    public async Task AccreditationBanner_WithTooLargeData_IsInvalid()
    {
        byte[] tooLarge = new byte[Division.Rules.AccreditationBannerMaxBytes + 1];

        ValidationResult result = await ValidateCreateAsync(new CreateDivisionCommandBuilder()
            .WithAccreditationBanner(new ImageBannerDtoBuilder().WithData(tooLarge).Build())
            .Build());

        AssertInvalid(result, "Accreditation banner must not exceed 5 MB.");
    }

    [Fact]
    public async Task TextsCollection_CannotBeNull()
    {
        CreateDivisionCommand command = new CreateDivisionCommandBuilder().Build() with { Texts = null! };

        ValidationResult result = await ValidateCreateAsync(command);

        AssertInvalid(result, "Texts collection is required.");
    }

    [Fact]
    public async Task Texts_CannotContainDuplicateContentTemplateAndAudiencePairs()
    {
        ValidationResult result = await ValidateCreateAsync(new CreateDivisionCommandBuilder()
            .WithTexts(
                new TextContentDtoBuilder().Build(),
                new TextContentDtoBuilder().WithContent("Duplicate").Build())
            .Build());

        AssertInvalid(result, "Duplicate text content for the same template and audience.");
    }

    [Fact]
    public async Task Text_WithInactiveOrMissingContentTemplateId_IsInvalid()
    {
        ValidationResult result = await ValidateCreateAsync(new CreateDivisionCommandBuilder()
            .WithTexts(new TextContentDtoBuilder().WithContentTemplateId(999).Build())
            .Build());

        AssertInvalid(result, "ContentTemplateId must reference an active Division content template.");
    }

    [Fact]
    public async Task Text_WithInactiveOrMissingAudienceId_IsInvalid()
    {
        ValidationResult result = await ValidateCreateAsync(new CreateDivisionCommandBuilder()
            .WithTexts(new TextContentDtoBuilder().WithAudienceId(999).Build())
            .Build());

        AssertInvalid(result, "AudienceId must reference an active audience.");
    }

    [Fact]
    public async Task Text_WithActiveAudienceId_IsValid()
    {
        ValidationResult result = await ValidateCreateAsync(new CreateDivisionCommandBuilder()
            .WithTexts(new TextContentDtoBuilder().WithAudienceId(10).Build())
            .Build());

        Assert.True(result.IsValid);
    }

    [Fact]
    public async Task Text_WithContentFormatNoneAndNonEmptyContent_IsInvalid()
    {
        ValidationResult result = await ValidateCreateAsync(new CreateDivisionCommandBuilder()
            .WithTexts(new TextContentDtoBuilder().WithFormat(ContentFormat.None).Build())
            .Build());

        AssertInvalid(result, "Content format is inconsistent with content.");
    }

    [Fact]
    public async Task Text_WithEmptyContentAndNonNoneFormat_IsInvalid()
    {
        ValidationResult result = await ValidateCreateAsync(new CreateDivisionCommandBuilder()
            .WithTexts(new TextContentDtoBuilder().WithContent(" ").WithFormat(ContentFormat.PlainText).Build())
            .Build());

        AssertInvalid(result, "Content format is inconsistent with content.");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task UpdateDivisionCommand_WithIdLessThanOrEqualZero_IsInvalid(int id)
    {
        UpdateDivisionCommandValidator validator = new(ValidReferenceData());
        UpdateDivisionCommand command = new UpdateDivisionCommandBuilder().WithId(id).Build();

        ValidationResult result = await validator.ValidateAsync(command);

        AssertInvalid(result, "Division id is required.");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task UpdateDivisionCommand_WithMissingVersion_IsInvalid(string? version)
    {
        ValidationResult result = await ValidateUpdateAsync(new UpdateDivisionCommandBuilder()
            .WithVersion(version)
            .Build());

        AssertInvalid(result, "Version is required.");
    }

    [Theory]
    [InlineData("abc")]
    [InlineData("not-base64")]
    [InlineData("AQID")]
    public async Task UpdateDivisionCommand_WithInvalidVersion_IsInvalid(string version)
    {
        ValidationResult result = await ValidateUpdateAsync(new UpdateDivisionCommandBuilder()
            .WithVersion(version)
            .Build());

        AssertInvalid(result, "Version must be a valid row version token.");
    }

    [Fact]
    public async Task UpdateDivisionCommand_WithValidRowVersion_IsValid()
    {
        string version = Convert.ToBase64String(new byte[8]);

        ValidationResult result = await ValidateUpdateAsync(new UpdateDivisionCommandBuilder()
            .WithVersion(version)
            .Build());

        Assert.True(result.IsValid);
    }

    private static Task<ValidationResult> ValidateCreateAsync(CreateDivisionCommand command)
    {
        CreateDivisionCommandValidator validator = new(ValidReferenceData());
        return validator.ValidateAsync(command);
    }

    private static Task<ValidationResult> ValidateUpdateAsync(UpdateDivisionCommand command)
    {
        UpdateDivisionCommandValidator validator = new(ValidReferenceData());
        return validator.ValidateAsync(command);
    }

    private static ReferenceDataValidationQueryFake ValidReferenceData()
        => new ReferenceDataValidationQueryFake()
            .WithActiveCountries(1)
            .WithActiveAudiences(10)
            .WithActiveContentTemplates(ContentTemplateScope.Division, 100);

    private static void AssertInvalid(ValidationResult result, string expectedMessagePart)
    {
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.ErrorMessage.Contains(expectedMessagePart, StringComparison.Ordinal));
    }
}
