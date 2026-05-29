using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Abstractions;
using ProductsAndPricingNew.Application.Common.Errors;
using ProductsAndPricingNew.Application.Features.ProductCategory.Abstractions;
using ProductsAndPricingNew.Domain.Common.Text;
using ProductsAndPricingNew.Domain.Repositories;
using ProductCategoryEntity = ProductsAndPricingNew.Domain.Entities.PricingRef.ProductCategory;

namespace ProductsAndPricingNew.Application.Features.ProductCategory.Commands.CreateProductCategory;

internal sealed class CreateProductCategoryCommandHandler : IRequestHandler<CreateProductCategoryCommand, Result<int>>
{
    private readonly IProductCategoryRepository _repository;
    private readonly IProductCategoryQuery _query;
    private readonly IUnitOfWork _unitOfWork;

    public CreateProductCategoryCommandHandler(
        IProductCategoryRepository repository,
        IProductCategoryQuery query,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _query = query;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<int>> Handle(CreateProductCategoryCommand request, CancellationToken ct)
    {
        string name = request.Name.AsRequiredText(nameof(request.Name));

        bool isNameTaken = await _query.ExistsByNameAsync(name, request.DivisionId, ct: ct);
        if (isNameTaken)
            return Result.Fail(new ConflictError($"Product category name: '{name}' already exists in this division"));

        ProductCategoryEntity category = ProductCategoryEntity.Create(request.DivisionId, name, request.IsActive);

        await _repository.AddAsync(category, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return Result.Ok(category.Id);
    }
}
