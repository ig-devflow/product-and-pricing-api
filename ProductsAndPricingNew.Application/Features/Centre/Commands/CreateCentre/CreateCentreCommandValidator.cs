using ProductsAndPricingNew.Application.Common.Validation.Abstractions;
using ProductsAndPricingNew.Application.Features.Centre.Validation;

namespace ProductsAndPricingNew.Application.Features.Centre.Commands.CreateCentre;

internal sealed class CreateCentreCommandValidator : CentreCommandValidatorBase<CreateCentreCommand>
{
    public CreateCentreCommandValidator(IReferenceDataValidationQuery referenceData) : base(referenceData)
    {
    }
}