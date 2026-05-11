using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Features.ReferenceData.Abstractions;
using ProductsAndPricingNew.Application.Features.ReferenceData.Models;

namespace ProductsAndPricingNew.Application.Features.ReferenceData.Queries.GetContentTemplates;

internal sealed class GetContentTemplatesQueryHandler : IRequestHandler<GetContentTemplatesQuery, Result<IReadOnlyCollection<ContentTemplateReferenceDto>>>
{
    private readonly IReferenceDataQuery _referenceDataQuery;

    public GetContentTemplatesQueryHandler(IReferenceDataQuery referenceDataQuery)
    {
        _referenceDataQuery = referenceDataQuery;
    }

    public async Task<Result<IReadOnlyCollection<ContentTemplateReferenceDto>>> Handle(GetContentTemplatesQuery request, CancellationToken ct)
    {
        IReadOnlyCollection<ContentTemplateReferenceDto> result = await _referenceDataQuery.GetContentTemplatesAsync(request.Scope, ct);

        return Result.Ok(result);
    }
}
