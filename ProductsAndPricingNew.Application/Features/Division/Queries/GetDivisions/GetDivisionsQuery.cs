using MediatR;
using ProductsAndPricingNew.Application.Features.Division.Dtos;

namespace ProductsAndPricingNew.Application.Features.Division.Queries.GetDivisions;

public sealed record GetDivisionsQuery(
    string? Search = null,
    bool? IsActive = null,
    bool? ShowInDropdown = null,
    int? Page = null,
    int? PageSize = null
) : IRequest<IReadOnlyList<DivisionListItemDto>>;