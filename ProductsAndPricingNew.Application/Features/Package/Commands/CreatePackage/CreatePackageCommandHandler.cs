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

namespace ProductsAndPricingNew.Application.Features.Package.Commands.CreatePackage;

internal sealed class CreatePackageCommandHandler : IRequestHandler<CreatePackageCommand, Result<int>>
{
    private readonly IPackageQuery _packageQuery;
    private readonly IPackageRepository _packageRepository;
    private readonly IUnitTypeProvider _unitTypeProvider;
    private readonly IUnitOfWork _unitOfWork;

    public CreatePackageCommandHandler(
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

    public async Task<Result<int>> Handle(CreatePackageCommand request, CancellationToken ct)
    {
        string name = request.Name.AsRequiredText(nameof(request.Name));

        bool isNameTaken = await _packageQuery.ExistsByNameAsync(name, ct: ct);
        if (isNameTaken)
            return Result.Fail(new ConflictError($"Package name: '{name}' already exists"));

        UnitType unitType = _unitTypeProvider.Get(request.UnitTypeId);

        IEnumerable<PackageItemDefinition> items = request.Items
            .Select(i => new PackageItemDefinition(i.ProductKind, i.ProductId, i.PriceBreakdown));

        PackageEntity package = new PackageEntity.Builder(request.DivisionId, name, unitType)
            .SetIsActive(request.IsActive)
            .WithDescription(request.Description)
            .WithCommission(request.Commission)
            .WithAgeRange(request.AgeFrom, request.AgeTo)
            .WithMinimumWeeks(request.MinimumWeeks)
            .WithItems(items)
            .WithCategories(request.AccountCategoryId, request.ProductCategoryId)
            .WithFinanceCodes(request.GeneralLedgerCode, request.CostCentreCode)
            .WithClosurePolicy(request.ClosurePolicy)
            .Build();

        package.EnsureBreakdownTotalEquals100();

        await _packageRepository.AddAsync(package, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return Result.Ok(package.Id);
    }
}
