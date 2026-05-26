using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Features.ReferenceData.Abstractions;
using ProductsAndPricingNew.Application.Features.ReferenceData.Models;

namespace ProductsAndPricingNew.Application.Features.ReferenceData.Queries.GetPrintFormats;

internal sealed class GetPrintFormatsQueryHandler : IRequestHandler<GetPrintFormatsQuery, Result<IReadOnlyCollection<PrintFormatReferenceDto>>>
{
    private readonly IReferenceDataQuery _referenceDataQuery;

    public GetPrintFormatsQueryHandler(IReferenceDataQuery referenceDataQuery)
    {
        _referenceDataQuery = referenceDataQuery;
    }

    public async Task<Result<IReadOnlyCollection<PrintFormatReferenceDto>>> Handle(GetPrintFormatsQuery request, CancellationToken ct)
    {
        IReadOnlyCollection<PrintFormatReferenceDto> result = await _referenceDataQuery.GetPrintFormatsAsync(ct);
        return Result.Ok(result);
    }
}