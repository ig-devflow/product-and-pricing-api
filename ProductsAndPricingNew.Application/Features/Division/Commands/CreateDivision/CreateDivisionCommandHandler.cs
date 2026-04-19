using MediatR;
using ProductsAndPricingNew.Domain.Entities.Common;
using ProductsAndPricingNew.Domain.Repositories;
using DivisionEntity = ProductsAndPricingNew.Domain.Entities.PricingRef.Division;

namespace ProductsAndPricingNew.Application.Features.Division.Commands.CreateDivision;

internal sealed class CreateDivisionCommandHandler : IRequestHandler<CreateDivisionCommand, int>
{
    private readonly IDivisionRepository _divisionRepository;

    public CreateDivisionCommandHandler(IDivisionRepository divisionRepository)
    {
        _divisionRepository = divisionRepository;
    }

    public async Task<int> Handle(CreateDivisionCommand request, CancellationToken ct)
    {
        Address? address = null;

        if (request.ContactAddress is not null)
        {
            address = new Address(
                request.ContactAddress.CountryId,
                request.ContactAddress.Line1,
                request.ContactAddress.Line2,
                request.ContactAddress.Line3,
                request.ContactAddress.Line4);
        }

        var division = new DivisionEntity.Builder(request.Name)
            .ShowInDropdown(request.ShowInDropdown)
            .IsActive(request.IsActive)
            .TermsAndConditions(request.TermsAndConditions)
            .GroupsPaymentTerms(request.GroupsPaymentTerms)
            .Website(request.WebsiteUrl)
            .HeadOfficeEmail(request.HeadOfficeEmail)
            .HeadOfficeTelephone(request.HeadOfficeTelephoneNo)
            .Address(address)
            .Build();

        await _divisionRepository.AddAsync(division, ct);

        return division.Id;
    }
}