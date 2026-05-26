using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Abstractions;
using ProductsAndPricingNew.Application.Common.Errors;
using ProductsAndPricingNew.Application.Features.AccommodationRoom.Abstractions;
using ProductsAndPricingNew.Application.Features.AccommodationRoom.Mappings;
using ProductsAndPricingNew.Domain.Common.Text;
using ProductsAndPricingNew.Domain.Repositories;
using ProductsAndPricingNew.Domain.UnitOfMeasure;
using AccommodationRoomEntity = ProductsAndPricingNew.Domain.Entities.Products.AccommodationRoom;

namespace ProductsAndPricingNew.Application.Features.AccommodationRoom.Commands.CreateAccommodationRoom;

internal sealed class CreateAccommodationRoomCommandHandler : IRequestHandler<CreateAccommodationRoomCommand, Result<int>>
{
    private readonly IAccommodationRoomQuery _accommodationRoomQuery;
    private readonly IAccommodationRoomRepository _accommodationRoomRepository;
    private readonly IUnitTypeProvider _unitTypeProvider;
    private readonly IUnitOfWork _unitOfWork;

    public CreateAccommodationRoomCommandHandler(
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

    public async Task<Result<int>> Handle(CreateAccommodationRoomCommand request, CancellationToken ct)
    {
        string name = request.Name.AsRequiredText(nameof(request.Name));

        bool isNameTaken = await _accommodationRoomQuery.ExistsByNameAsync(name, ct: ct);
        if (isNameTaken)
            return Result.Fail(new ConflictError($"Accommodation room name: '{name}' already exists"));

        UnitType unitType = _unitTypeProvider.Get(request.UnitTypeId);

        AccommodationRoomEntity accommodation = new AccommodationRoomEntity.Builder(request.AccommodationId, request.DivisionId, unitType, name)
            .SetIsActive(request.IsActive)
            .SetOccupyRoom(request.OccupyRoom)
            .WithRoomDetails(request.RoomDetails.ToDefinition())
            .WithCategories(request.AccountCategoryId, request.ProductCategoryId)
            .WithFinanceCodes(request.GeneralLedgerCode, request.CostCentreCode)
            .WithClosurePolicy(request.ClosurePolicy)
            .Build();

        await _accommodationRoomRepository.AddAsync(accommodation, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return Result.Ok(accommodation.Id);
    }
}