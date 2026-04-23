using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Features.Division.Abstractions;
using ProductsAndPricingNew.Application.Features.Division.Models;
using ProductsAndPricingNew.Domain.Entities.Common;
using ProductsAndPricingNew.Domain.Repositories;
using DivisionEntity = ProductsAndPricingNew.Domain.Entities.PricingRef.Division;

namespace ProductsAndPricingNew.Application.Features.Division.Commands.UpdateDivision;

internal sealed class UpdateDivisionCommandHandler : IRequestHandler<UpdateDivisionCommand, Result<Unit>>
{
    private readonly IDivisionRepository _divisionRepository;
    private readonly IDivisionQueries _divisionQueries;

    public UpdateDivisionCommandHandler(IDivisionRepository divisionRepository, IDivisionQueries divisionQueries)
    {
        _divisionRepository = divisionRepository;
        _divisionQueries = divisionQueries;
    }

    public async Task<Result<Unit>> Handle(UpdateDivisionCommand request, CancellationToken ct)
    {
        DivisionEntity? division = await _divisionRepository.GetByIdAsync(request.Id, ct);
        if (division is null)
            return Result.Fail($"Division with id {request.Id} was not found");

        if (await _divisionQueries.ExistsByNameAsync(request.Name.Trim(), request.Id, ct))
            return Result.Fail("Division name already exists");

        DivisionAddressDto? addressDto = request.ContactAddress;
        DivisionAccreditationBanner? banner = request.AccreditationBanner;

        Address address = addressDto is not null
            ? Address.Create(addressDto.CountryId, addressDto.Street, addressDto.District, addressDto.City, addressDto.PostalCode)
            : Address.Empty;

        ImageFile imageFile = banner is not null
            ? ImageFile.Create(banner.Data, banner.ContentType, banner.FileName)
            : ImageFile.Empty;

        division.Rename(request.Name);
        division.ChangeActiveState(request.IsActive);
        division.ChangeTermsAndConditions(request.TermsAndConditions);
        division.ChangeGroupsPaymentTerms(request.GroupsPaymentTerms);
        division.ChangeWebsite(request.WebsiteUrl);
        division.ChangeHeadOfficeEmail(request.HeadOfficeEmail);
        division.ChangeHeadOfficeTelephone(request.HeadOfficeTelephoneNo);
        division.ChangeContactAddress(address);
        division.ChangeAccreditationBanner(imageFile);

        return Result.Ok(Unit.Value);
    }
}