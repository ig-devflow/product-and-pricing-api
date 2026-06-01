using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Common.Errors;
using ProductsAndPricingNew.Application.Features.AccountCategory.Abstractions;
using ProductsAndPricingNew.Application.Features.AccountCategory.Models;

namespace ProductsAndPricingNew.Application.Features.AccountCategory.Queries.GetAccountCategoryById;

internal sealed class GetAccountCategoryByIdQueryHandler : IRequestHandler<GetAccountCategoryByIdQuery, Result<AccountCategoryDetailsDto>>
{
    private readonly IAccountCategoryQuery _query;

    public GetAccountCategoryByIdQueryHandler(IAccountCategoryQuery query)
    {
        _query = query;
    }

    public async Task<Result<AccountCategoryDetailsDto>> Handle(GetAccountCategoryByIdQuery request, CancellationToken ct)
    {
        AccountCategoryDetailsDto? result = await _query.GetByIdAsync(request.Id, ct);
        if (result is null)
            return Result.Fail(new NotFoundError($"Account category with id {request.Id} was not found"));

        return Result.Ok(result);
    }
}
