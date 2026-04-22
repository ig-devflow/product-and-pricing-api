using MediatR;
using ProductsAndPricingNew.Domain.Entities.Common;
using ProductsAndPricingNew.Domain.Repositories;
using DivisionEntity = ProductsAndPricingNew.Domain.Entities.PricingRef.Division;

namespace ProductsAndPricingNew.Application.Features.Division.Commands.UpdateDivision;

internal sealed class UpdateDivisionCommandHandler : IRequestHandler<UpdateDivisionCommand, Unit>
{
    private readonly IDivisionRepository _divisionRepository;

    public UpdateDivisionCommandHandler(IDivisionRepository divisionRepository)
    {
        _divisionRepository = divisionRepository;
    }

    public async Task<Unit> Handle(UpdateDivisionCommand request, CancellationToken ct)
    {
        DivisionEntity? division = await _divisionRepository.GetByIdAsync(request.Id, ct);
        if (division is null)
            throw new KeyNotFoundException($"Division with id {request.Id} was not found");

        division.Rename(request.Name);

        if (request.IsActive)
            division.Activate();
        else
            division.Deactivate();

        division.ChangeTermsAndConditions(request.TermsAndConditions);
        division.ChangeGroupsPaymentTerms(request.GroupsPaymentTerms);
        division.ChangeWebsite(request.WebsiteUrl);
        division.ChangeHeadOfficeEmail(request.HeadOfficeEmail);
        division.ChangeHeadOfficeTelephone(request.HeadOfficeTelephoneNo);

        Address address = request.ContactAddress is null
            ? Address.Empty
            : new(
                request.ContactAddress.CountryId,
                request.ContactAddress.Line1,
                request.ContactAddress.Line2,
                request.ContactAddress.Line3,
                request.ContactAddress.Line4);

        division.ChangeContactAddress(address);

        return Unit.Value;
    }
}