using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Abstractions;
using ProductsAndPricingNew.Application.Common.Errors;
using ProductsAndPricingNew.Application.Features.ProductCategory.Abstractions;
using ProductsAndPricingNew.Domain.Common.Text;
using ProductsAndPricingNew.Domain.Repositories;
using ProductCategoryEntity = ProductsAndPricingNew.Domain.Entities.PricingRef.ProductCategory;

namespace ProductsAndPricingNew.Application.Features.ProductCategory.Commands.UpdateProductCategory;

internal sealed class UpdateProductCategoryCommandHandler : IRequestHandler<UpdateProductCategoryCommand, Result<Unit>>
{
    private readonly IProductCategoryRepository _repository;
    private readonly IProductCategoryQuery _query;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProductCategoryCommandHandler(
        IProductCategoryRepository repository,
        IProductCategoryQuery query,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _query = query;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Unit>> Handle(UpdateProductCategoryCommand request, CancellationToken ct)
    {
        ProductCategoryEntity? category = await _repository.GetByIdAsync(request.Id, ct);
        if (category is null)
            return Result.Fail(new NotFoundError($"Product category with id {request.Id} was not found"));

        if (!category.HasVersion(request.Version))
            return Result.Fail(new ConflictError("Product category was modified by another user. Reload it and try again."));

        string name = request.Name.AsRequiredText(nameof(request.Name));

        bool isNameTaken = await _query.ExistsByNameAsync(name, category.DivisionId, request.Id, ct);
        if (isNameTaken)
            return Result.Fail(new ConflictError($"Product category name: '{name}' already exists in this division"));

        category.Rename(name);

        await _unitOfWork.SaveChangesAsync(ct);

        return Result.Ok();
    }
}
