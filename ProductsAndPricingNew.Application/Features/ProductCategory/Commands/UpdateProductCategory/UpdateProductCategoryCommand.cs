using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Abstractions;

namespace ProductsAndPricingNew.Application.Features.ProductCategory.Commands.UpdateProductCategory;

public sealed record UpdateProductCategoryCommand(
    int Id,
    string Name,
    string Version
) : ICommand<Result<Unit>>;
