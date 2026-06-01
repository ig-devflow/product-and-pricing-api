using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Common.Errors;
using ProductsAndPricingNew.Application.Features.ProductCategory.Abstractions;
using ProductsAndPricingNew.Application.Features.ProductCategory.Models;

namespace ProductsAndPricingNew.Application.Features.ProductCategory.Queries.GetProductCategoryById;

internal sealed class GetProductCategoryByIdQueryHandler : IRequestHandler<GetProductCategoryByIdQuery, Result<ProductCategoryDetailsDto>>
{
    private readonly IProductCategoryQuery _query;

    public GetProductCategoryByIdQueryHandler(IProductCategoryQuery query)
    {
        _query = query;
    }

    public async Task<Result<ProductCategoryDetailsDto>> Handle(GetProductCategoryByIdQuery request, CancellationToken ct)
    {
        ProductCategoryDetailsDto? result = await _query.GetByIdAsync(request.Id, ct);
        if (result is null)
            return Result.Fail(new NotFoundError($"Product category with id {request.Id} was not found"));

        return Result.Ok(result);
    }
}
