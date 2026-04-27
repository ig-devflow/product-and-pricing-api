using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Abstractions;
using ProductsAndPricingNew.Application.Common.Errors;
using ProductsAndPricingNew.Application.Common.Models;
using ProductsAndPricingNew.Application.Features.Division.Abstractions;
using ProductsAndPricingNew.Application.Features.Division.Models;
using ProductsAndPricingNew.Domain.Common.Extensions;
using ProductsAndPricingNew.Domain.Entities.Common;
using ProductsAndPricingNew.Domain.Repositories;
using DivisionEntity = ProductsAndPricingNew.Domain.Entities.PricingRef.Division;

namespace ProductsAndPricingNew.Application.Features.Division.Commands.UpdateDivision;

internal sealed class UpdateDivisionCommandHandler : IRequestHandler<UpdateDivisionCommand, Result<Unit>>
{
    private readonly IDivisionRepository _divisionRepository;
    private readonly IDivisionQueries _divisionQueries;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateDivisionCommandHandler(
        IDivisionRepository divisionRepository,
        IDivisionQueries divisionQueries,
        IUnitOfWork unitOfWork)
    {
        _divisionRepository = divisionRepository;
        _divisionQueries = divisionQueries;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Unit>> Handle(UpdateDivisionCommand request, CancellationToken ct)
    {
        DivisionEntity? division = await _divisionRepository.GetByIdAsync(request.Id, ct);
        if (division is null)
            return Result.Fail(new NotFoundError($"Division with id {request.Id} was not found"));

        string name = request.Name.AsRequiredDomainText();

        bool nameAlreadyExists = await _divisionQueries.ExistsByNameAsync(name, request.Id, ct);
        if (nameAlreadyExists)
            return Result.Fail(new ConflictError("Division name already exists"));

        AddressDto? addressDto = request.ContactAddress;
        ImageBannerDto? banner = request.AccreditationBanner;

        Address address = addressDto is not null
            ? Address.Create(addressDto.CountryId, addressDto.Street, addressDto.District, addressDto.City, addressDto.PostalCode)
            : Address.Empty;

        ImageFile imageFile = banner is not null
            ? ImageFile.Create(banner.Data, banner.ContentType, banner.FileName)
            : ImageFile.Empty;

        division.Rename(name);
        division.ChangeActiveState(request.IsActive);
        division.ChangeTermsAndConditions(request.TermsAndConditions);
        division.ChangeGroupsPaymentTerms(request.GroupsPaymentTerms);
        division.ChangeWebsite(request.WebsiteUrl);
        division.ChangeHeadOfficeEmail(request.HeadOfficeEmail);
        division.ChangeHeadOfficeTelephone(request.HeadOfficeTelephoneNo);
        division.ChangeContactAddress(address);
        division.ChangeAccreditationBanner(imageFile);

        await _unitOfWork.SaveChangesAsync(ct);

        return Result.Ok(Unit.Value);
    }
}