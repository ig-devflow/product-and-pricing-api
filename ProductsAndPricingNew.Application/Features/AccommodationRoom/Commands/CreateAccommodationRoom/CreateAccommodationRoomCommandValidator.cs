using FluentValidation;
using ProductsAndPricingNew.Application.Common.Validation.Abstractions;
using ProductsAndPricingNew.Application.Features.AccommodationRoom.Validation;

namespace ProductsAndPricingNew.Application.Features.AccommodationRoom.Commands.CreateAccommodationRoom;

internal sealed class CreateAccommodationRoomCommandValidator : AccommodationRoomCommandValidatorBase<CreateAccommodationRoomCommand>
{
    public CreateAccommodationRoomCommandValidator(IReferenceDataValidationQuery referenceData) : base(referenceData)
    {
        RuleFor(x => x.DivisionId)
            .GreaterThan(0) // todo: referenceData
            .WithMessage("DivisionId is required.");

        RuleFor(x => x.AccommodationId)
            .GreaterThan(0) // todo: referenceData
            .WithMessage("AccommodationId is required.");
    }
}