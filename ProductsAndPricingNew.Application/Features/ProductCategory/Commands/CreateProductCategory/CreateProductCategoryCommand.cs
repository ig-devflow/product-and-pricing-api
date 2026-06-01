using FluentResults;
using ProductsAndPricingNew.Application.Abstractions;

namespace ProductsAndPricingNew.Application.Features.ProductCategory.Commands.CreateProductCategory;

public sealed record CreateProductCategoryCommand(
    int DivisionId,
    string Name,
    bool IsActive
) : ICommand<Result<int>>;
