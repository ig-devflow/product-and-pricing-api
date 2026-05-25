using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Abstractions;
using ProductsAndPricingNew.Application.Common.Errors;
using ProductsAndPricingNew.Application.Features.Package.Abstractions;
using ProductsAndPricingNew.Domain.Common.Text;
using ProductsAndPricingNew.Domain.Entities.Products.Definitions;
using ProductsAndPricingNew.Domain.Repositories;
using ProductsAndPricingNew.Domain.UnitOfMeasure;
using PackageEntity = ProductsAndPricingNew.Domain.Entities.Products.Package;

namespace ProductsAndPricingNew.Application.Features.Package.Commands.UpdatePackage;

internal sealed class UpdatePackageCommandHandler : IRequestHandler<UpdatePackageCommand, Result<Unit>>
{
    private readonly IPackageQuery _packageQuery;
    private readonly IPackageRepository _packageRepository;
    private readonly IUnitTypeProvider _unitTypeProvider;
    private readonly IUnitOfWork _unitOfWork;

    public UpdatePackageCommandHandler(
        IPackageQuery packageQuery,
        IPackageRepository packageRepository,
        IUnitTypeProvider unitTypeProvider,
        IUnitOfWork unitOfWork)
    {
        _packageQuery = packageQuery;
        _packageRepository = packageRepository;
        _unitTypeProvider = unitTypeProvider;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Unit>> Handle(UpdatePackageCommand request, CancellationToken ct)
    {
        PackageEntity? package = await _packageRepository.GetByIdAsync(request.Id, ct);
        if (package is null)
            return Result.Fail(new NotFoundError($"Package with id {request.Id} was not found"));

        if (!package.HasVersion(request.Version))
            return Result.Fail(new ConflictError("Package was modified by another user. Reload it and try again."));

        string name = request.Name.AsRequiredText(nameof(request.Name));

        bool isNameTaken = await _packageQuery.ExistsByNameAsync(name, request.Id, ct);
        if (isNameTaken)
            return Result.Fail(new ConflictError($"Package name: '{name}' already exists"));

        UnitType unitType = _unitTypeProvider.Get(request.UnitTypeId);

        package.Rename(name);
        package.SetIsActive(request.IsActive);
        package.WithUnitType(unitType);
        package.WithDescription(request.Description);
        package.WithCommission(request.Commission);
        package.WithAgeRange(request.AgeFrom, request.AgeTo);
        package.WithMinimumWeeks(request.MinimumWeeks);
        package.WithCategories(request.AccountCategoryId, request.ProductCategoryId);
        package.WithFinanceCodes(request.GeneralLedgerCode, request.CostCentreCode);
        package.WithClosurePolicy(request.ClosurePolicy);
        package.WithItems(request.PackageItems.Select(i => new PackageItemDefinition(i.ProductKind, i.ProductId, i.PriceBreakdown)));
        package.EnsureBreakdownTotalEquals100();

        await _unitOfWork.SaveChangesAsync(ct);

        return Result.Ok();
    }
}
