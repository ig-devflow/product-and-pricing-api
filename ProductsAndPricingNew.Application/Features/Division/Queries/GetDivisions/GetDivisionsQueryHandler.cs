using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductsAndPricingNew.Application.Features.Division.Dtos;
using ProductsAndPricingNew.Persistence;
using DivisionEntity = ProductsAndPricingNew.Domain.Entities.PricingRef.Division;

namespace ProductsAndPricingNew.Application.Features.Division.Queries.GetDivisions;

internal sealed class GetDivisionsQueryHandler : IRequestHandler<GetDivisionsQuery, IReadOnlyList<DivisionListItemDto>>
{
    private readonly ProductsAndPricingDbContext _db;

    public GetDivisionsQueryHandler(ProductsAndPricingDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<DivisionListItemDto>> Handle(GetDivisionsQuery request, CancellationToken ct)
    {
        IQueryable<DivisionEntity> query = _db.Divisions.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.Trim();
            query = query.Where(x => x.Name.Contains(search));
        }

        if (request.IsActive.HasValue)
            query = query.Where(x => x.IsActive == request.IsActive.Value);

        if (request.ShowInDropdown.HasValue)
            query = query.Where(x => x.ShowInDropdown == request.ShowInDropdown.Value);

        query = query.OrderBy(x => x.Name).ThenBy(x => x.Id);

        if (request.Page.HasValue && request.PageSize.HasValue)
        {
            var page = request.Page.Value > 0 ? request.Page.Value : 1;
            var pageSize = request.PageSize.Value > 0 ? request.PageSize.Value : 20;

            query = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize);
        }

        return await query
            .Select(x => new DivisionListItemDto(
                x.Id,
                x.Name,
                x.ShowInDropdown,
                x.IsActive))
            .ToListAsync(ct);
    }
}