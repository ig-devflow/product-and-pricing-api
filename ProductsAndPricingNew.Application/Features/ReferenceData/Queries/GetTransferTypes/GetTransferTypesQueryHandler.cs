using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Features.ReferenceData.Abstractions;
using ProductsAndPricingNew.Application.Features.ReferenceData.Models;

namespace ProductsAndPricingNew.Application.Features.ReferenceData.Queries.GetTransferTypes;

internal sealed class GetTransferTypesQueryHandler : IRequestHandler<GetTransferTypesQuery, Result<IReadOnlyCollection<TransferTypeReferenceDto>>>
{
    private readonly IReferenceDataQuery _referenceDataQuery;

    public GetTransferTypesQueryHandler(IReferenceDataQuery referenceDataQuery)
    {
        _referenceDataQuery = referenceDataQuery;
    }

    public async Task<Result<IReadOnlyCollection<TransferTypeReferenceDto>>> Handle(GetTransferTypesQuery request, CancellationToken ct)
    {
        IReadOnlyCollection<TransferTypeReferenceDto> result = await _referenceDataQuery.GetTransferTypesAsync(ct);
        return Result.Ok(result);
    }
}
