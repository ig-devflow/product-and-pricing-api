using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Features.AccountCategory.Models;

namespace ProductsAndPricingNew.Application.Features.AccountCategory.Queries.GetAccountCategoryById;

public sealed record GetAccountCategoryByIdQuery(int Id) : IRequest<Result<AccountCategoryDetailsDto>>;
