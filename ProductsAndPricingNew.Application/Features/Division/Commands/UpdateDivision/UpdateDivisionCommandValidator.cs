using FluentValidation;
using ProductsAndPricingNew.Application.Common.Validation.Abstractions;
using ProductsAndPricingNew.Application.Features.Division.Validation;

namespace ProductsAndPricingNew.Application.Features.Division.Commands.UpdateDivision;

internal sealed class UpdateDivisionCommandValidator : DivisionCommandValidatorBase<UpdateDivisionCommand>
{
    public UpdateDivisionCommandValidator(IReferenceDataValidationQuery referenceData) : base(referenceData)
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Division id is required.");

        RuleFor(x => x.Version)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Version is required.")
            .Must(BeValidRowVersion)
            .WithMessage("Version must be a valid row version token.");
    }

    private static bool BeValidRowVersion(string? version)
    {
        if (string.IsNullOrWhiteSpace(version))
            return false;

        try
        {
            byte[] bytes = Convert.FromBase64String(version);
            return bytes.Length == 8;
        }
        catch (FormatException)
        {
            return false;
        }
    }
}