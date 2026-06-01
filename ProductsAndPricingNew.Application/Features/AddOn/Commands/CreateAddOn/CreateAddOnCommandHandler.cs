using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Abstractions;
using ProductsAndPricingNew.Application.Common.Errors;
using ProductsAndPricingNew.Application.Features.AddOn.Abstractions;
using ProductsAndPricingNew.Domain.Common.Text;
using ProductsAndPricingNew.Domain.Repositories;
using ProductsAndPricingNew.Domain.UnitOfMeasure;
using AddOnEntity = ProductsAndPricingNew.Domain.Entities.Products.AddOn;

namespace ProductsAndPricingNew.Application.Features.AddOn.Commands.CreateAddOn;

internal sealed class CreateAddOnCommandHandler : IRequestHandler<CreateAddOnCommand, Result<int>>
{
    private readonly IAddOnQuery _addOnQuery;
    private readonly IAddOnRepository _addOnRepository;
    private readonly IUnitTypeProvider _unitTypeProvider;
    private readonly IUnitOfWork _unitOfWork;

    public CreateAddOnCommandHandler(
        IAddOnQuery addOnQuery,
        IAddOnRepository addOnRepository,
        IUnitTypeProvider unitTypeProvider,
        IUnitOfWork unitOfWork)
    {
        _addOnQuery = addOnQuery;
        _addOnRepository = addOnRepository;
        _unitOfWork = unitOfWork;
        _unitTypeProvider = unitTypeProvider;
    }

    public async Task<Result<int>> Handle(CreateAddOnCommand request, CancellationToken ct)
    {
        string name = request.Name.AsRequiredText(nameof(request.Name));

        bool isNameTaken = await _addOnQuery.ExistsByNameAsync(name, ct: ct);
        if (isNameTaken)
            return Result.Fail(new ConflictError($"Add on name: '{name}' already exists"));

        UnitType unitType = _unitTypeProvider.Get(request.UnitTypeId);

        AddOnEntity addOn = new AddOnEntity.Builder(request.DivisionId, name, request.AddOnType, unitType)
            .SetIsActive(request.IsActive)
            .WithAgeRange(request.AgeFrom, request.AgeTo)
            .WithCategories(request.AccountCategoryId, request.ProductCategoryId)
            .WithOneToOneLessonsPerWeek(request.OneToOneLessonsPerWeek)
            .WithFinanceCodes(request.GeneralLedgerCode, request.CostCentreCode)
            .WithClosurePolicy(request.ClosurePolicy)
            .Build();

        await _addOnRepository.AddAsync(addOn, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return Result.Ok(addOn.Id);
    }
}