using FluentValidation;
using ProductsAndPricingNew.Application.Common.Validation.Abstractions;
using ProductsAndPricingNew.Application.Features.AccommodationRoom.Abstractions;
using ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;
using AccommodationRoomAggregate = ProductsAndPricingNew.Domain.Entities.Products.AccommodationRoom;

namespace ProductsAndPricingNew.Application.Features.AccommodationRoom.Validation;

internal abstract class AccommodationRoomCommandValidatorBase<TCommand> : AbstractValidator<TCommand>
    where TCommand : IAccommodationRoomCommandPayload
{
    protected AccommodationRoomCommandValidatorBase(IReferenceDataValidationQuery referenceDataValidation)
    {
        RuleFor(x => x.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Accommodation room name is required.")
            .MaximumLength(AccommodationRoomAggregate.Rules.NameMaxLength)
            .WithMessage($"Accommodation room name must not exceed {AccommodationRoomAggregate.Rules.NameMaxLength} characters.");

        RuleFor(x => x.UnitTypeId)
            .Cascade(CascadeMode.Stop)
            .GreaterThan(0)
            .WithMessage("UnitTypeId is required.")
            .MustAsync((unitTypeId, ct) => UnitTypeTypeIsActiveAsync(referenceDataValidation, unitTypeId, ct))
            .WithMessage("UnitTypeId must reference an active unit type.");

        RuleFor(x => x.RoomDetails.RoomTypeId)
            .Cascade(CascadeMode.Stop)
            .GreaterThan(0)
            .WithMessage("RoomTypeId is required.")
            .MustAsync((roomTypeId, ct) => AccommodationRoomTypeIsActiveAsync(referenceDataValidation, roomTypeId, ct))
            .WithMessage("RoomTypeId must reference an active accommodation room type.");

        RuleFor(x => x.RoomDetails.BoardTypeId)
            .Cascade(CascadeMode.Stop)
            .GreaterThan(0)
            .WithMessage("BoardTypeId is required.")
            .MustAsync((boardTypeId, ct) => AccommodationBoardTypeIsActiveAsync(referenceDataValidation, boardTypeId, ct))
            .WithMessage("BoardTypeId must reference an active accommodation board type.");

        RuleFor(x => x.RoomDetails.BathroomTypeId)
            .Cascade(CascadeMode.Stop)
            .GreaterThan(0)
            .WithMessage("BathroomTypeId is required.")
            .MustAsync((bathroomTypeId, ct) => AccommodationBathroomTypeIsActiveAsync(referenceDataValidation, bathroomTypeId, ct))
            .WithMessage("BathroomTypeId must reference an active accommodation bathroom type.");

        RuleFor(x => x.RoomDetails.RoomGradeId)
            .Cascade(CascadeMode.Stop)
            .GreaterThan(0)
            .WithMessage("RoomGradeId is required.")
            .MustAsync((roomGradeId, ct) => AccommodationRoomGradeIsActiveAsync(referenceDataValidation, roomGradeId, ct))
            .WithMessage("RoomGradeId must reference an active accommodation room grade.");

        RuleFor(x => x.AccountCategoryId) //todo: referenceData
            .Cascade(CascadeMode.Stop)
            .GreaterThan(0)
            .WithMessage("AccountCategoryId is required.");

        RuleFor(x => x.ProductCategoryId) //todo: referenceData
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

    private static async Task<bool> UnitTypeTypeIsActiveAsync(IReferenceDataValidationQuery referenceData, int unitTypeId, CancellationToken ct)
    {
        if (unitTypeId <= 0)
            return false;

        IReadOnlySet<int> unitTypesIds = await referenceData.GetActiveUnitTypesIdsAsync(new[] { unitTypeId }, ct);
        return unitTypesIds.Contains(unitTypeId);
    }

    private static async Task<bool> AccommodationRoomTypeIsActiveAsync(IReferenceDataValidationQuery referenceData, int roomTypeId, CancellationToken ct)
    {
        if (roomTypeId <= 0)
            return false;

        IReadOnlySet<int> roomTypesIds = await referenceData.GetActiveAccommodationRoomTypesIdsAsync(new[] { roomTypeId }, ct);
        return roomTypesIds.Contains(roomTypeId);
    }

    private static async Task<bool> AccommodationBoardTypeIsActiveAsync(IReferenceDataValidationQuery referenceData, int boardTypeId, CancellationToken ct)
    {
        if (boardTypeId <= 0)
            return false;

        IReadOnlySet<int> boardTypesIds = await referenceData.GetActiveAccommodationBoardTypesIdsAsync(new[] { boardTypeId }, ct);
        return boardTypesIds.Contains(boardTypeId);
    }

    private static async Task<bool> AccommodationBathroomTypeIsActiveAsync(IReferenceDataValidationQuery referenceData, int bathroomTypeId, CancellationToken ct)
    {
        if (bathroomTypeId <= 0)
            return false;

        IReadOnlySet<int> bathroomTypeIds = await referenceData.GetActiveAccommodationBathroomTypesIdsAsync(new[] { bathroomTypeId }, ct);
        return bathroomTypeIds.Contains(bathroomTypeId);
    }

    private static async Task<bool> AccommodationRoomGradeIsActiveAsync(IReferenceDataValidationQuery referenceData, int roomGradeId, CancellationToken ct)
    {
        if (roomGradeId <= 0)
            return false;

        IReadOnlySet<int> roomGradesIds = await referenceData.GetActiveAccommodationRoomGradesIdsAsync(new[] { roomGradeId }, ct);
        return roomGradesIds.Contains(roomGradeId);
    }
}