using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Features.AccountCategory.Abstractions;
using ProductsAndPricingNew.Application.Features.AccountCategory.Models;

namespace ProductsAndPricingNew.Application.Features.AccountCategory.Queries.GetAccountCategories;

internal sealed class GetAccountCategoriesQueryHandler : IRequestHandler<GetAccountCategoriesQuery, Result<IReadOnlyCollection<AccountCategoryListItemDto>>>
{
    private readonly IAccountCategoryQuery _query;

    public GetAccountCategoriesQueryHandler(IAccountCategoryQuery query)
    {
        _query = query;
    }

    public async Task<Result<IReadOnlyCollection<AccountCategoryListItemDto>>> Handle(GetAccountCategoriesQuery request, CancellationToken ct)
    {
        IReadOnlyCollection<AccountCategoryListItemDto> result = await _query.GetListByDivisionAsync(request.DivisionId, ct);
        return Result.Ok(result);
    }
}
