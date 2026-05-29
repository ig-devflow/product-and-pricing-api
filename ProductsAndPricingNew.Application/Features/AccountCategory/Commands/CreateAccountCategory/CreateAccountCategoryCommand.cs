using FluentResults;
using ProductsAndPricingNew.Application.Abstractions;

namespace ProductsAndPricingNew.Application.Features.AccountCategory.Commands.CreateAccountCategory;

public sealed record CreateAccountCategoryCommand(
    int DivisionId,
    string Name,
    bool IsActive
) : ICommand<Result<int>>;
