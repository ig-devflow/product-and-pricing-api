using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Abstractions;
using ProductsAndPricingNew.Application.Common.Errors;
using ProductsAndPricingNew.Application.Features.AddOn.Abstractions;
using ProductsAndPricingNew.Domain.Common.Text;
using ProductsAndPricingNew.Domain.Repositories;
using ProductsAndPricingNew.Domain.UnitOfMeasure;
using AddOnEntity = ProductsAndPricingNew.Domain.Entities.Products.AddOn;

namespace ProductsAndPricingNew.Application.Features.AddOn.Commands.UpdateAddOn;

internal sealed class UpdateAddOnCommandHandler : IRequestHandler<UpdateAddOnCommand, Result<Unit>>
{
    private readonly IAddOnQuery _addOnQuery;
    private readonly IAddOnRepository _addOnRepository;
    private readonly IUnitTypeProvider _unitTypeProvider;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateAddOnCommandHandler(
        IAddOnQuery addOnQuery,
        IAddOnRepository addOnRepository,
        IUnitTypeProvider unitTypeProvider,
        IUnitOfWork unitOfWork)
    {
        _addOnQuery = addOnQuery;
        _addOnRepository = addOnRepository;
        _unitTypeProvider = unitTypeProvider;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Unit>> Handle(UpdateAddOnCommand request, CancellationToken ct)
    {
        AddOnEntity? addOn = await _addOnRepository.GetByIdAsync(request.Id, ct);
        if (addOn is null)
            return Result.Fail(new NotFoundError($"Add on with id {request.Id} was not found"));

        if (!addOn.HasVersion(request.Version))
            return Result.Fail(new ConflictError("Add on was modified by another user. Reload it and try again."));

        string name = request.Name.AsRequiredText(nameof(request.Name));

        bool isNameTaken = await _addOnQuery.ExistsByNameAsync(name, request.Id, ct);
        if (isNameTaken)
            return Result.Fail(new ConflictError($"Add on name: '{name}' already exists"));

        UnitType unitType = _unitTypeProvider.Get(request.UnitTypeId);

        addOn.Rename(name);
        addOn.SetIsActive(request.IsActive);
        addOn.WithType(request.AddOnType);
        addOn.WithUnitType(unitType);
        addOn.WithType(request.AddOnType);
        addOn.WithAgeRange(request.AgeFrom, request.AgeTo);
        addOn.WithOneToOneLessonsPerWeek(request.OneToOneLessonsPerWeek);
        addOn.WithCategories(request.AccountCategoryId, request.ProductCategoryId);
        addOn.WithFinanceCodes(request.GeneralLedgerCode, request.CostCentreCode);
        addOn.WithClosurePolicy(request.ClosurePolicy);

        await _unitOfWork.SaveChangesAsync(ct);

        return Result.Ok();
    }
}