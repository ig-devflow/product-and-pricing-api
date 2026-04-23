using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Features.Division.Dtos;
using ProductsAndPricingNew.Domain.Entities.Common;
using ProductsAndPricingNew.Domain.Repositories;
using DivisionEntity = ProductsAndPricingNew.Domain.Entities.PricingRef.Division;

namespace ProductsAndPricingNew.Application.Features.Division.Commands.CreateDivision;

internal sealed class CreateDivisionCommandHandler : IRequestHandler<CreateDivisionCommand, Result<int>>
{
    private readonly IDivisionRepository _divisionRepository;

    public CreateDivisionCommandHandler(IDivisionRepository divisionRepository)
    {
        _divisionRepository = divisionRepository;
    }

    public async Task<Result<int>> Handle(CreateDivisionCommand request, CancellationToken ct)
    {
        // if (await _divisionRepository.ExistsAsync(request.Name, ct))
        //     return -1;

        DivisionAddressDto? addressDto = request.ContactAddress;
        Address? address = addressDto is not null
            ? new Address(addressDto.CountryId, addressDto.Line1, addressDto.Line2, addressDto.Line3, addressDto.Line4)
            : Address.Empty;

        DivisionEntity division = new DivisionEntity.Builder(request.Name)
            .IsActive(request.IsActive)
            .TermsAndConditions(request.TermsAndConditions)
            .GroupsPaymentTerms(request.GroupsPaymentTerms)
            .Website(request.WebsiteUrl)
            .HeadOfficeEmail(request.HeadOfficeEmail)
            .HeadOfficeTelephone(request.HeadOfficeTelephoneNo)
            .Address(address)
            .Build();

        await _divisionRepository.AddAsync(division, ct);

        return Result.Ok(division.Id);
    }
}