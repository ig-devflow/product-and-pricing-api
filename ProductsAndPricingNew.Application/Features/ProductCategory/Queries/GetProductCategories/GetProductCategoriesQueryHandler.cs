using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Features.ProductCategory.Abstractions;
using ProductsAndPricingNew.Application.Features.ProductCategory.Models;

namespace ProductsAndPricingNew.Application.Features.ProductCategory.Queries.GetProductCategories;

internal sealed class GetProductCategoriesQueryHandler : IRequestHandler<GetProductCategoriesQuery, Result<IReadOnlyCollection<ProductCategoryListItemDto>>>
{
    private readonly IProductCategoryQuery _query;

    public GetProductCategoriesQueryHandler(IProductCategoryQuery query)
    {
        _query = query;
    }

    public async Task<Result<IReadOnlyCollection<ProductCategoryListItemDto>>> Handle(GetProductCategoriesQuery request, CancellationToken ct)
    {
        IReadOnlyCollection<ProductCategoryListItemDto> result = await _query.GetListByDivisionAsync(request.DivisionId, ct);
        return Result.Ok(result);
    }
}
