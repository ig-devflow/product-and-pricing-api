using MediatR;
using ProductsAndPricingNew.Application.Features.Division.Abstractions;
using ProductsAndPricingNew.Application.Features.Division.Dtos;

namespace ProductsAndPricingNew.Application.Features.Division.Queries.GetDivisionById;

internal sealed class GetDivisionByIdQueryHandler : IRequestHandler<GetDivisionByIdQuery, DivisionDetailsDto?>
{
    private readonly IDivisionQueries _divisionQueries;

    public GetDivisionByIdQueryHandler(IDivisionQueries divisionQueries)
    {
        _divisionQueries = divisionQueries;
    }

    public Task<DivisionDetailsDto?> Handle(GetDivisionByIdQuery request, CancellationToken ct)
        => _divisionQueries.GetByIdAsync(request.Id, ct);
}