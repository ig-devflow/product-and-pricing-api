using ProductsAndPricingNew.Application.Common.Validation;
using ProductsAndPricingNew.Application.Common.Validation.Abstractions;
using ProductsAndPricingNew.Application.Features.Division.Abstractions;

namespace ProductsAndPricingNew.Application.Features.Division.Commands.CreateDivision;

internal sealed class CreateDivisionCommandValidator : DivisionCommandValidatorBase<CreateDivisionCommand>
{
    public CreateDivisionCommandValidator(IReferenceDataValidationQuery referenceData) : base(referenceData)
    {
    }
}