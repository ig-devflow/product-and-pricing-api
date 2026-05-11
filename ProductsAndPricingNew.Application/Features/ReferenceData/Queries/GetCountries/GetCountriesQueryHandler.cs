using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Features.ReferenceData.Abstractions;
using ProductsAndPricingNew.Application.Features.ReferenceData.Models;

namespace ProductsAndPricingNew.Application.Features.ReferenceData.Queries.GetCountries;

internal sealed class GetCountriesQueryHandler : IRequestHandler<GetCountriesQuery, Result<IReadOnlyCollection<CountryReferenceDto>>>
{
    private readonly IReferenceDataQuery _referenceDataQuery;

    public GetCountriesQueryHandler(IReferenceDataQuery referenceDataQuery)
    {
        _referenceDataQuery = referenceDataQuery;
    }

    public async Task<Result<IReadOnlyCollection<CountryReferenceDto>>> Handle(GetCountriesQuery request, CancellationToken ct)
    {
        IReadOnlyCollection<CountryReferenceDto> result = await _referenceDataQuery.GetCountriesAsync(ct);

        return Result.Ok(result);
    }
}
