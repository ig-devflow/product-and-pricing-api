using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Features.Division.Abstractions;
using ProductsAndPricingNew.Application.Features.Division.Models;
using ProductsAndPricingNew.Domain.Entities.Common;
using ProductsAndPricingNew.Domain.Repositories;
using DivisionEntity = ProductsAndPricingNew.Domain.Entities.PricingRef.Division;

namespace ProductsAndPricingNew.Application.Features.Division.Commands.CreateDivision;

internal sealed class CreateDivisionCommandHandler : IRequestHandler<CreateDivisionCommand, Result<int>>
{
    private readonly IDivisionRepository _divisionRepository;
    private readonly IDivisionQueries _divisionQueries;

    public CreateDivisionCommandHandler(IDivisionRepository divisionRepository, IDivisionQueries divisionQueries)
    {
        _divisionRepository = divisionRepository;
        _divisionQueries = divisionQueries;
    }

    public async Task<Result<int>> Handle(CreateDivisionCommand request, CancellationToken ct)
    {
        if (await _divisionQueries.ExistsByNameAsync(request.Name.Trim(), ct: ct))
            return Result.Fail("Division name already exists");

        DivisionAddressDto? addressDto = request.ContactAddress;
        DivisionAccreditationBanner? banner = request.AccreditationBanner;

        Address address = addressDto is not null
            ? Address.Create(addressDto.CountryId, addressDto.Street, addressDto.District, addressDto.City, addressDto.PostalCode)
            : Address.Empty;

        ImageFile imageFile = banner is not null
            ? ImageFile.Create(banner.Data, banner.ContentType, banner.FileName)
            : ImageFile.Empty;

        DivisionEntity division = new DivisionEntity.Builder(request.Name, request.WebsiteUrl)
            .IsActive(request.IsActive)
            .TermsAndConditions(request.TermsAndConditions)
            .GroupsPaymentTerms(request.GroupsPaymentTerms)
            .HeadOfficeEmail(request.HeadOfficeEmail)
            .HeadOfficeTelephone(request.HeadOfficeTelephoneNo)
            .ContactAddress(address)
            .AccreditationBanner(imageFile)
            .Build();

        await _divisionRepository.AddAsync(division, ct);

        return Result.Ok(division.Id);
    }
}