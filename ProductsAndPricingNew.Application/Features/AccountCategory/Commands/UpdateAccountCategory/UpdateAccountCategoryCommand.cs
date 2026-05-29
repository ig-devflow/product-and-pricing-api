using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Abstractions;

namespace ProductsAndPricingNew.Application.Features.AccountCategory.Commands.UpdateAccountCategory;

public sealed record UpdateAccountCategoryCommand(
    int Id,
    string Name,
    string Version
) : ICommand<Result<Unit>>;
