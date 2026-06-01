using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Features.ProductCategory.Models;

namespace ProductsAndPricingNew.Application.Features.ProductCategory.Queries.GetProductCategories;

public sealed record GetProductCategoriesQuery(int DivisionId) : IRequest<Result<IReadOnlyCollection<ProductCategoryListItemDto>>>;
