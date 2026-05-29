using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Features.ProductCategory.Models;

namespace ProductsAndPricingNew.Application.Features.ProductCategory.Queries.GetProductCategoryById;

public sealed record GetProductCategoryByIdQuery(int Id) : IRequest<Result<ProductCategoryDetailsDto>>;
