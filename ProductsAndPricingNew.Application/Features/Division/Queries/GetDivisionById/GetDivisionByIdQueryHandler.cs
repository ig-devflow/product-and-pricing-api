using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductsAndPricingNew.Application.Features.Division.Dtos;
using ProductsAndPricingNew.Persistence;

namespace ProductsAndPricingNew.Application.Features.Division.Queries.GetDivisionById;

internal sealed class GetDivisionByIdQueryHandler : IRequestHandler<GetDivisionByIdQuery, DivisionDetailsDto?>
{
    private readonly ProductsAndPricingDbContext _db;

    public GetDivisionByIdQueryHandler(ProductsAndPricingDbContext db)
    {
        _db = db;
    }

    public async Task<DivisionDetailsDto?> Handle(GetDivisionByIdQuery request, CancellationToken ct)
    {
        return await _db.Divisions
            .AsNoTracking()
            .Where(x => x.Id == request.Id)
            .Select(x => new DivisionDetailsDto( // automapper
                x.Id,
                x.Name,
                x.ShowInDropdown,
                x.IsActive,
                x.TermsAndConditions,
                x.GroupsPaymentTerms,
                x.WebsiteUrl,
                x.HeadOfficeEmail,
                x.HeadOfficeTelephoneNo,
                x.ContactAddress == null
                    ? null
                    : new DivisionAddressDto(
                        x.ContactAddress.Line1,
                        x.ContactAddress.Line2,
                        x.ContactAddress.Line3,
                        x.ContactAddress.Line4,
                        x.ContactAddress.CountryId)))
            .FirstOrDefaultAsync(ct);
    }
}