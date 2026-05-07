using FluentValidation;
using ProductsAndPricingNew.Application.Common.Models;
using ProductsAndPricingNew.Application.Common.Validation.Abstractions;

namespace ProductsAndPricingNew.Application.Common.Validation.Validators;

internal sealed class AddressDtoValidator : AbstractValidator<AddressDto>
{
    private const int AddressFieldMaxLength = 50;

    public AddressDtoValidator(IReferenceDataValidationQuery referenceData, bool requireCountry = true)
    {
        if (requireCountry)
        {
            RuleFor(x => x.CountryId)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .WithMessage("CountryId is required when address is provided.")
                .GreaterThan(0)
                .WithMessage("CountryId must be greater than 0.")
                .MustAsync((countryId, ct) => CountryExistsAsync(referenceData, countryId, ct))
                .WithMessage("CountryId must reference an existing country.");
        }
        else
        {
            RuleFor(x => x.CountryId)
                .Cascade(CascadeMode.Stop)
                .GreaterThan(0)
                .WithMessage("CountryId must be greater than 0.")
                .MustAsync((countryId, ct) => CountryExistsAsync(referenceData, countryId, ct))
                .WithMessage("CountryId must reference an existing country.")
                .When(x => x.CountryId.HasValue);
        }

        RuleFor(x => x.Street)
            .MaximumLength(AddressFieldMaxLength)
            .WithMessage($"Street must not exceed {AddressFieldMaxLength} characters.");

        RuleFor(x => x.District)
            .MaximumLength(AddressFieldMaxLength)
            .WithMessage($"District must not exceed {AddressFieldMaxLength} characters.");

        RuleFor(x => x.City)
            .MaximumLength(AddressFieldMaxLength)
            .WithMessage($"City must not exceed {AddressFieldMaxLength} characters.");

        RuleFor(x => x.PostalCode)
            .MaximumLength(AddressFieldMaxLength)
            .WithMessage($"Postal code must not exceed {AddressFieldMaxLength} characters.");
    }

    private static async Task<bool> CountryExistsAsync(IReferenceDataValidationQuery referenceData, int? countryId, CancellationToken ct)
    {
        if (countryId is null or <= 0)
            return true;

        IReadOnlySet<int> existingCountryIds = await referenceData.GetExistingCountryIdsAsync(new[] { countryId.Value }, ct);

        return existingCountryIds.Contains(countryId.Value);
    }
}