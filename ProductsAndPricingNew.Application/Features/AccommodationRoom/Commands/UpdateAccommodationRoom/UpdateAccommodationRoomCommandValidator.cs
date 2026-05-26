using FluentValidation;
using ProductsAndPricingNew.Application.Common.Validation.Abstractions;
using ProductsAndPricingNew.Application.Common.Validation.Extensions;
using ProductsAndPricingNew.Application.Features.AccommodationRoom.Validation;

namespace ProductsAndPricingNew.Application.Features.AccommodationRoom.Commands.UpdateAccommodationRoom;

internal sealed class UpdateAccommodationRoomCommandValidator : AccommodationRoomCommandValidatorBase<UpdateAccommodationRoomCommand>
{
    public UpdateAccommodationRoomCommandValidator(IReferenceDataValidationQuery referenceDataValidation) : base(referenceDataValidation)
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("AccommodationRoom id is required.");

        RuleFor(x => x.Version)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Version is required.")
            .IsValidRowVersion()
            .WithMessage("Version must be a valid row version token.");
    }
}