using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Abstractions;
using ProductsAndPricingNew.Application.Common.Errors;
using ProductsAndPricingNew.Application.Features.Accommodation.Abstractions;
using ProductsAndPricingNew.Domain.Common.Text;
using ProductsAndPricingNew.Domain.Repositories;
using ProductsAndPricingNew.Domain.SharedKernel.Definitions;
using AccommodationEntity = ProductsAndPricingNew.Domain.Entities.Products.Accommodation;

namespace ProductsAndPricingNew.Application.Features.Accommodation.Commands.CreateAccommodation;

internal sealed class CreateAccommodationCommandHandler : IRequestHandler<CreateAccommodationCommand, Result<int>>
{
    private readonly IAccommodationQuery _accommodationQuery;
    private readonly IAccommodationRepository _accommodationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateAccommodationCommandHandler(
        IAccommodationQuery accommodationQuery,
        IAccommodationRepository accommodationRepository,
        IUnitOfWork unitOfWork)
    {
        _accommodationQuery = accommodationQuery;
        _accommodationRepository = accommodationRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<int>> Handle(CreateAccommodationCommand request, CancellationToken ct)
    {
        string name = request.Name.AsRequiredText(nameof(request.Name));

        bool isNameTaken = await _accommodationQuery.ExistsByNameAsync(name, ct: ct);
        if (isNameTaken)
            return Result.Fail(new ConflictError($"Accommodation name: '{name}' already exists"));

        AccommodationEntity accommodation = new AccommodationEntity.Builder(name, request.AccommodationTypeId)
            .WithMinimumStayInWeeks(request.MinimumStayInWeeks)
            .IsActive(request.IsActive)
            .WithAgeRange(new AgeRangeDefinition(request.AgeFrom, request.AgeTo))
            .WithCommitment(request.IsCommitted, request.IsNonCommitted)
            .Build();

        await _accommodationRepository.AddAsync(accommodation, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return Result.Ok(accommodation.Id);
    }
}