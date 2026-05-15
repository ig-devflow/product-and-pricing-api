using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Abstractions;
using ProductsAndPricingNew.Application.Common.Errors;
using ProductsAndPricingNew.Application.Common.Mapping;
using ProductsAndPricingNew.Application.Features.Centre.Abstractions;
using ProductsAndPricingNew.Application.Features.Centre.Mappings;
using ProductsAndPricingNew.Domain.Repositories;
using CentreEntity = ProductsAndPricingNew.Domain.Entities.PricingRef.Centre;

namespace ProductsAndPricingNew.Application.Features.Centre.Commands.UpdateCentre;

internal sealed class UpdateCentreCommandHandler : IRequestHandler<UpdateCentreCommand, Result<Unit>>
{
    private readonly ICentreRepository _centreRepository;
    private readonly ICentreQuery _centreQuery;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCentreCommandHandler(
        ICentreRepository centreRepository,
        ICentreQuery centreQuery,
        IUnitOfWork unitOfWork)
    {
        _centreRepository = centreRepository;
        _centreQuery = centreQuery;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Unit>> Handle(UpdateCentreCommand request, CancellationToken ct)
    {
        CentreEntity? centre = await _centreRepository.GetByIdWithTextsAsync(request.Id, ct);
        if (centre is null)
            return Result.Fail(new NotFoundError($"Centre with id {request.Id} was not found"));

        if (!centre.HasVersion(request.Version))
            return Result.Fail(new ConflictError("Centre was modified by another user. Reload it and try again."));

        bool isNameTaken = await _centreQuery.ExistsByNameAsync(request.Name, request.Id, ct);
        if (isNameTaken)
            return Result.Fail(new ConflictError($"Centre name: '{request.Name}' already exists"));

        var contactInfo = request.ContactInfo;
        var legalInfo = request.LegalInfo;
        var ratios = request.OperationalRatios;

        centre.Rename(request.Name);
        centre.ChangeCode(request.Code);
        centre.ChangeCurrency(request.CurrencyId);
        centre.ChangePrintFormat(request.PrintFormat);
        centre.ChangeActive(request.IsActive);
        centre.ChangePhysicalCentre(request.IsPhysicalCentre);
        centre.ChangeGeneralEmail(contactInfo.GeneralEmail);
        centre.ChangeAccommodationEmail(contactInfo.AccommodationEmail);
        centre.ChangeTelephone(contactInfo.Telephone);
        centre.ChangeEmergencyTelephone(contactInfo.EmergencyTelephone);
        centre.ChangeTransferEmergencyTelephone(contactInfo.TransferEmergencyTelephone);
        centre.ChangeBrandColor(contactInfo.BrandColor);
        centre.ChangeContactAddress(contactInfo.ContactAddress.ToDefinition());
        centre.ChangeLogo(contactInfo.LogoImage.ToDefinition());
        centre.ChangeSchoolSponsorshipNumber(legalInfo.SchoolSponsorshipNumber);
        centre.ChangeVatNumber(legalInfo.VatNumber);
        centre.ChangeRegistrationNumber(legalInfo.RegistrationNumber);
        centre.ChangeVatExemptionNumber(legalInfo.VatExemptionNumber);
        centre.ChangeChequePayableTo(legalInfo.ChequePayableTo);
        centre.ChangeGuarantees(ratios.Guarantees);
        centre.ChangeIndividualsRatio(ratios.IndividualsRatio);
        centre.ChangeStaffingRatio(ratios.StaffingRatio);
        centre.ChangeEmptyBeds(ratios.EmptyBeds);
        centre.ChangeBankDetails(request.BankDetails.ToDefinition());
        centre.ReplaceContacts(request.Contacts.ToDefinitions());
        centre.ReplaceTexts(request.Texts.ToDefinitions());

        await _centreRepository.AddAsync(centre, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return Result.Ok();
    }
}