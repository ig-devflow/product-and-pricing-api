using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Features.ReferenceData.Abstractions;
using ProductsAndPricingNew.Application.Features.ReferenceData.Models;

namespace ProductsAndPricingNew.Application.Features.ReferenceData.Queries.GetCurrencies;

internal sealed class GetCurrenciesQueryHandler : IRequestHandler<GetCurrenciesQuery, Result<IReadOnlyCollection<CurrencyReferenceDto>>>
{
    private readonly IReferenceDataQuery _referenceDataQuery;

    public GetCurrenciesQueryHandler(IReferenceDataQuery referenceDataQuery)
    {
        _referenceDataQuery = referenceDataQuery;
    }

    public async Task<Result<IReadOnlyCollection<CurrencyReferenceDto>>> Handle(GetCurrenciesQuery request, CancellationToken ct)
    {
        IReadOnlyCollection<CurrencyReferenceDto> result = await _referenceDataQuery.GetCurrenciesAsync(ct);

        return Result.Ok(result);
    }
}
