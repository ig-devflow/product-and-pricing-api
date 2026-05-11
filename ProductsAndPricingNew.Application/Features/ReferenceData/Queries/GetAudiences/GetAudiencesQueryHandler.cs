using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Features.ReferenceData.Abstractions;
using ProductsAndPricingNew.Application.Features.ReferenceData.Models;

namespace ProductsAndPricingNew.Application.Features.ReferenceData.Queries.GetAudiences;

internal sealed class GetAudiencesQueryHandler : IRequestHandler<GetAudiencesQuery, Result<IReadOnlyCollection<AudienceReferenceDto>>>
{
    private readonly IReferenceDataQuery _referenceDataQuery;

    public GetAudiencesQueryHandler(IReferenceDataQuery referenceDataQuery)
    {
        _referenceDataQuery = referenceDataQuery;
    }

    public async Task<Result<IReadOnlyCollection<AudienceReferenceDto>>> Handle(GetAudiencesQuery request, CancellationToken ct)
    {
        IReadOnlyCollection<AudienceReferenceDto> result = await _referenceDataQuery.GetAudiencesAsync(ct);

        return Result.Ok(result);
    }
}
