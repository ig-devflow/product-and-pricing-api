using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Abstractions;
using ProductsAndPricingNew.Application.Common.Errors;
using ProductsAndPricingNew.Application.Features.AccommodationRoom.Abstractions;
using ProductsAndPricingNew.Application.Features.AccommodationRoom.Mappings;
using ProductsAndPricingNew.Domain.Common.Text;
using ProductsAndPricingNew.Domain.Repositories;
using ProductsAndPricingNew.Domain.SharedKernel.Definitions;
using ProductsAndPricingNew.Domain.UnitOfMeasure;
using AccommodationRoomEntity = ProductsAndPricingNew.Domain.Entities.Products.AccommodationRoom;

namespace ProductsAndPricingNew.Application.Features.AccommodationRoom.Commands.UpdateAccommodationRoom;

internal sealed class UpdateAccommodationRoomCommandHandler : IRequestHandler<UpdateAccommodationRoomCommand , Result<Unit>>
{
    private readonly IAccommodationRoomQuery _accommodationRoomQuery;
    private readonly IAccommodationRoomRepository _accommodationRoomRepository;
    private readonly IUnitTypeProvider _unitTypeProvider;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateAccommodationRoomCommandHandler(
        IAccommodationRoomQuery accommodationRoomQuery,
        IAccommodationRoomRepository accommodationRoomRepository,
        IUnitTypeProvider unitTypeProvider,
        IUnitOfWork unitOfWork)
    {
        _accommodationRoomQuery = accommodationRoomQuery;
        _accommodationRoomRepository = accommodationRoomRepository;
        _unitTypeProvider = unitTypeProvider;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result<Unit>> Handle(UpdateAccommodationRoomCommand request, CancellationToken ct)
    {
        AccommodationRoomEntity? accommodationRoom = await _accommodationRoomRepository.GetByIdAsync(request.Id, ct);
        if (accommodationRoom is null)
            return Result.Fail(new NotFoundError($"Accommodation room with id {request.Id} was not found"));

        if (!accommodationRoom.HasVersion(request.Version))
            return Result.Fail(new ConflictError("Accommodation room was modified by another user. Reload it and try again."));

        string name = request.Name.AsRequiredText(nameof(request.Name));

        bool isNameTaken = await _accommodationRoomQuery.ExistsByNameAsync(name, request.Id, ct);
        if (isNameTaken)
            return Result.Fail(new ConflictError($"Accommodation room name: '{name}' already exists"));
        
        UnitType unitType = _unitTypeProvider.Get(request.UnitTypeId);
        
        accommodationRoom.Rename(name);
        accommodationRoom.ChangeUnitType(unitType);
        accommodationRoom.SetIsActive(request.IsActive);
        accommodationRoom.SetOccupyRoom(request.OccupyRoom);
        accommodationRoom.ChangeRoomDetails(request.roomDetails.ToDefinition());
        accommodationRoom.ChangeCategories(new ProductCategoriesDefinition(request.AccountCategoryId, request.ProductCategoryId));
        accommodationRoom.ChangeFinanceCodes(new FinanceCodesDefinition(request.GeneralLedgerCode, request.CostCentreCode));
        accommodationRoom.ChangeClosurePolicy(request.ClosurePolicy);
        
        await _unitOfWork.SaveChangesAsync(ct);

        return Result.Ok();
    }
}