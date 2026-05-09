using FluentValidation;
using ProductsAndPricingNew.Application.Common.Models;
using ProductsAndPricingNew.Application.Common.Validation.Abstractions;
using ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;

namespace ProductsAndPricingNew.Application.Common.Validation.Validators;

internal sealed class AddressDtoValidator : AbstractValidator<AddressDto>
{
    public AddressDtoValidator(IReferenceDataValidationQuery referenceData)
    {
        When(HasAnyAddressField, () =>
        {
            RuleFor(x => x.CountryId)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .WithMessage("CountryId is required when address is provided.")
                .GreaterThan(0)
                .WithMessage("CountryId must be greater than 0.")
                .MustAsync((countryId, ct) => CountryIsActiveAsync(referenceData, countryId, ct))
                .WithMessage("CountryId must reference an active country.");
        });

        RuleFor(x => x.Street)
            .MaximumLength(Address.Rules.AddressFieldMaxLength)
            .WithMessage($"Street must not exceed {Address.Rules.AddressFieldMaxLength} characters.");

        RuleFor(x => x.District)
            .MaximumLength(Address.Rules.AddressFieldMaxLength)
            .WithMessage($"District must not exceed {Address.Rules.AddressFieldMaxLength} characters.");

        RuleFor(x => x.City)
            .MaximumLength(Address.Rules.AddressFieldMaxLength)
            .WithMessage($"City must not exceed {Address.Rules.AddressFieldMaxLength} characters.");

        RuleFor(x => x.PostalCode)
            .MaximumLength(Address.Rules.AddressFieldMaxLength)
            .WithMessage($"Postal code must not exceed {Address.Rules.AddressFieldMaxLength} characters.");
    }

    private static bool HasAnyAddressField(AddressDto address)
    {
        return address.CountryId.HasValue
            || !string.IsNullOrWhiteSpace(address.Street)
            || !string.IsNullOrWhiteSpace(address.District)
            || !string.IsNullOrWhiteSpace(address.City)
            || !string.IsNullOrWhiteSpace(address.PostalCode);
    }

    private static async Task<bool> CountryIsActiveAsync(IReferenceDataValidationQuery referenceData, int? countryId, CancellationToken ct)
    {
        if (countryId is null or <= 0)
            return true;

        IReadOnlySet<int> activeCountryIds = await referenceData.GetActiveCountryIdsAsync(new[] { countryId.Value }, ct);

        return activeCountryIds.Contains(countryId.Value);
    }
}
