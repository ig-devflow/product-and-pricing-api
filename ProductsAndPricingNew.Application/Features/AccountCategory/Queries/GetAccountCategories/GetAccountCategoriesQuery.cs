using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Features.AccountCategory.Models;

namespace ProductsAndPricingNew.Application.Features.AccountCategory.Queries.GetAccountCategories;

public sealed record GetAccountCategoriesQuery(int DivisionId) : IRequest<Result<IReadOnlyCollection<AccountCategoryListItemDto>>>;
