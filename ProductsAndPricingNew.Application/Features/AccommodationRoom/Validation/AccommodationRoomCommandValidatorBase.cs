using FluentValidation;
using ProductsAndPricingNew.Application.Common.Validation.Abstractions;
using ProductsAndPricingNew.Application.Features.AccommodationRoom.Abstractions;
using ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;
using AccommodationRoomAggregate = ProductsAndPricingNew.Domain.Entities.Products.AccommodationRoom;

namespace ProductsAndPricingNew.Application.Features.AccommodationRoom.Validation;

internal abstract class AccommodationRoomCommandValidatorBase<TCommand> : AbstractValidator<TCommand>
    where TCommand : IAccommodationRoomCommandPayload
{
    protected AccommodationRoomCommandValidatorBase(IReferenceDataValidationQuery referenceData)
    {
        RuleFor(x => x.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Accommodation room name is required.")
            .MaximumLength(AccommodationRoomAggregate.Rules.NameMaxLength)
            .WithMessage($"Accommodation room name must not exceed {AccommodationRoomAggregate.Rules.NameMaxLength} characters.");

        RuleFor(x => x.UnitTypeId) // referenceData
            .Cascade(CascadeMode.Stop)
            .GreaterThan(0)
            .WithMessage("UnitTypeId is required.");

        RuleFor(x => x.RoomDetails.RoomTypeId) // referenceData
            .Cascade(CascadeMode.Stop)
            .GreaterThan(0)
            .WithMessage("RoomTypeId is required.");

        RuleFor(x => x.RoomDetails.BoardTypeId) // referenceData
            .Cascade(CascadeMode.Stop)
            .GreaterThan(0)
            .WithMessage("BoardTypeId is required.");

        RuleFor(x => x.RoomDetails.BathroomTypeId) // referenceData
            .Cascade(CascadeMode.Stop)
            .GreaterThan(0)
            .WithMessage("BathroomTypeId is required.");

        RuleFor(x => x.RoomDetails.RoomGradeId) // referenceData
            .Cascade(CascadeMode.Stop)
            .GreaterThan(0)
            .WithMessage("RoomGradeId is required.");

        RuleFor(x => x.AccountCategoryId) // referenceData
            .Cascade(CascadeMode.Stop)
            .GreaterThan(0)
            .WithMessage("AccountCategoryId is required.");

        RuleFor(x => x.ProductCategoryId) // referenceData
            .Cascade(CascadeMode.Stop)
            .GreaterThan(0)
            .WithMessage("ProductCategoryId is required.");

        RuleFor(x => x.GeneralLedgerCode)
            .MaximumLength(FinanceCodes.Rules.MaxLength)
            .WithMessage($"General ledger code must not exceed {FinanceCodes.Rules.MaxLength} characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.GeneralLedgerCode));

        RuleFor(x => x.CostCentreCode)
            .MaximumLength(FinanceCodes.Rules.MaxLength)
            .WithMessage($"Cost centre code must not exceed {FinanceCodes.Rules.MaxLength} characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.CostCentreCode));

        RuleFor(x => x.ClosurePolicy)
            .Must(OfferingsClosurePolicy.IsValid)
            .WithMessage("Closure policy date cannot be in the past.")
            .When(x => x.ClosurePolicy.HasValue);
    }
}