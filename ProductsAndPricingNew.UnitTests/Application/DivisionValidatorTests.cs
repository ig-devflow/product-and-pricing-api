using FluentValidation.Results;
using ProductsAndPricingNew.Application.Common.Models;
using ProductsAndPricingNew.Application.Common.Validation.Abstractions;
using ProductsAndPricingNew.Application.Common.Validation.Validators;
using ProductsAndPricingNew.Application.Features.Division.Commands.CreateDivision;
using ProductsAndPricingNew.Domain.Entities.PricingRef;
using ProductsAndPricingNew.Domain.ReferenceData;

namespace ProductsAndPricingNew.UnitTests.Application;

public sealed class DivisionValidatorTests
{
    [Fact]
    public async Task AddressValidator_WithEmptyAddressDto_IsValid()
    {
        AddressDtoValidator validator = new(new ReferenceDataValidationQueryStub());
        AddressDto address = new(null, null, null, null, null);

        ValidationResult result = await validator.ValidateAsync(address);

        Assert.True(result.IsValid);
    }

    [Fact]
    public async Task AddressValidator_WithPartialAddressWithoutCountryId_IsInvalid()
    {
        AddressDtoValidator validator = new(new ReferenceDataValidationQueryStub());
        AddressDto address = new("Street", null, null, null, null);

        ValidationResult result = await validator.ValidateAsync(address);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.ErrorMessage == "CountryId is required when address is provided.");
    }

    [Fact]
    public async Task AddressValidator_WithDeletedCountry_IsInvalid()
    {
        ReferenceDataValidationQueryStub referenceData = new();
        AddressDtoValidator validator = new(referenceData);
        AddressDto address = new("Street", null, null, null, 10);

        ValidationResult result = await validator.ValidateAsync(address);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.ErrorMessage == "CountryId must reference an active country.");
    }

    [Fact]
    public async Task DivisionValidator_WithDuplicateTextKeys_IsInvalid()
    {
        ReferenceDataValidationQueryStub referenceData = ValidReferenceData();
        CreateDivisionCommandValidator validator = new(referenceData);
        CreateDivisionCommand command = ValidCommand([
            new TextContentDto(1, null, "Public copy", ContentFormat.PlainText),
            new TextContentDto(1, null, "Duplicate public copy", ContentFormat.PlainText)
        ]);

        ValidationResult result = await validator.ValidateAsync(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.ErrorMessage == "Duplicate text content for the same template and audience.");
    }

    [Fact]
    public async Task DivisionValidator_UsesDivisionRulesForTermsAndConditionsMaxLength()
    {
        ReferenceDataValidationQueryStub referenceData = ValidReferenceData();
        CreateDivisionCommandValidator validator = new(referenceData);
        CreateDivisionCommand command = ValidCommand(
            termsAndConditions: new string('A', Division.Rules.TermsAndConditionsMaxLength + 1));

        ValidationResult result = await validator.ValidateAsync(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.ErrorMessage.Contains(Division.Rules.TermsAndConditionsMaxLength.ToString(), StringComparison.Ordinal));
    }

    [Fact]
    public async Task DivisionValidator_UsesDivisionRulesForGroupsPaymentTermsMaxLength()
    {
        ReferenceDataValidationQueryStub referenceData = ValidReferenceData();
        CreateDivisionCommandValidator validator = new(referenceData);
        CreateDivisionCommand command = ValidCommand(
            groupsPaymentTerms: new string('A', Division.Rules.GroupsPaymentTermsMaxLength + 1));

        ValidationResult result = await validator.ValidateAsync(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.ErrorMessage.Contains(Division.Rules.GroupsPaymentTermsMaxLength.ToString(), StringComparison.Ordinal));
    }

    private static ReferenceDataValidationQueryStub ValidReferenceData()
    {
        ReferenceDataValidationQueryStub referenceData = new();
        referenceData.ActiveCountryIds.Add(1);
        referenceData.ActiveAudienceIds.Add(1);
        referenceData.ActiveContentTemplateIds.Add(1);
        return referenceData;
    }

    private static CreateDivisionCommand ValidCommand(
        IReadOnlyCollection<TextContentDto>? texts = null,
        string? termsAndConditions = null,
        string? groupsPaymentTerms = null)
    {
        return new CreateDivisionCommand(
            "Division",
            "https://example.com",
            true,
            termsAndConditions,
            groupsPaymentTerms,
            "office@example.com",
            "+1 555 123 4567",
            new AddressDto("Street", null, "City", null, 1),
            new ImageBannerDto([1], "image/png", "banner.png"),
            texts ?? [new TextContentDto(1, null, "Public copy", ContentFormat.PlainText)]);
    }

    private sealed class ReferenceDataValidationQueryStub : IReferenceDataValidationQuery
    {
        public HashSet<int> ActiveCountryIds { get; } = new();
        public HashSet<int> ActiveAudienceIds { get; } = new();
        public HashSet<int> ActiveContentTemplateIds { get; } = new();

        public Task<IReadOnlySet<int>> GetActiveCountryIdsAsync(IReadOnlyCollection<int> ids, CancellationToken ct = default)
            => Task.FromResult(Filter(ids, ActiveCountryIds));

        public Task<IReadOnlySet<int>> GetActiveAudienceIdsAsync(IReadOnlyCollection<int> ids, CancellationToken ct = default)
            => Task.FromResult(Filter(ids, ActiveAudienceIds));

        public Task<IReadOnlySet<int>> GetActiveContentTemplateIdsAsync(
            IReadOnlyCollection<int> ids,
            ContentTemplateScope scope,
            CancellationToken ct = default)
            => Task.FromResult(Filter(ids, ActiveContentTemplateIds));

        private static IReadOnlySet<int> Filter(IReadOnlyCollection<int> ids, HashSet<int> activeIds)
        {
            return ids
                .Where(activeIds.Contains)
                .ToHashSet();
        }
    }
}
