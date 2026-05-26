using ProductsAndPricingNew.Application.Common.Validation.Abstractions;
using ProductsAndPricingNew.Application.Features.Accommodation.Validation;

namespace ProductsAndPricingNew.Application.Features.Accommodation.Commands.CreateAccommodation;

internal sealed class CreateAccommodationCommandValidator : AccommodationCommandValidatorBase<CreateAccommodationCommand>
{
    public CreateAccommodationCommandValidator(IReferenceDataValidationQuery referenceData) : base(referenceData)
    {
    }
}