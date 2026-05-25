using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Abstractions;
using ProductsAndPricingNew.Application.Common.Errors;
using ProductsAndPricingNew.Application.Features.Accommodation.Abstractions;
using ProductsAndPricingNew.Domain.Common.Text;
using ProductsAndPricingNew.Domain.Repositories;
using ProductsAndPricingNew.Domain.SharedKernel.Definitions;
using AccommodationEntity = ProductsAndPricingNew.Domain.Entities.Products.Accommodation;

namespace ProductsAndPricingNew.Application.Features.Accommodation.Commands.UpdateAccommodation;

internal sealed class UpdateAccommodationCommandHandler : IRequestHandler<UpdateAccommodationCommand, Result<Unit>>
{
    private readonly IAccommodationQuery _accommodationQuery;
    private readonly IAccommodationRepository _accommodationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateAccommodationCommandHandler(
        IAccommodationQuery accommodationQuery,
        IAccommodationRepository accommodationRepository,
        IUnitOfWork unitOfWork)
    {
        _accommodationQuery = accommodationQuery;
        _accommodationRepository = accommodationRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Unit>> Handle(UpdateAccommodationCommand request, CancellationToken ct)
    {
        AccommodationEntity? accommodation = await _accommodationRepository.GetByIdAsync(request.Id, ct);
        if (accommodation is null)
            return Result.Fail(new NotFoundError($"Accommodation with id {request.Id} was not found"));

        if (!accommodation.HasVersion(request.Version))
            return Result.Fail(new ConflictError("Accommodation was modified by another user. Reload it and try again."));

        string name = request.Name.AsRequiredText(nameof(request.Name));

        bool isNameTaken = await _accommodationQuery.ExistsByNameAsync(name, request.Id, ct);
        if (isNameTaken)
            return Result.Fail(new ConflictError($"Accommodation name: '{name}' already exists"));

        accommodation.Rename(name);
        accommodation.ChangeAccommodationType(request.AccommodationTypeId);
        accommodation.SetIsActive(request.IsActive);
        accommodation.ChangeMinimumStay(request.MinimumStayInWeeks);
        accommodation.ChangeAgeRange(new AgeRangeDefinition(request.AgeFrom, request.AgeTo));
        accommodation.ChangeCommitment(request.IsCommitted, request.IsNonCommitted);

        await _unitOfWork.SaveChangesAsync(ct);

        return Result.Ok();
    }
}