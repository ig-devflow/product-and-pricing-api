using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Abstractions;
using ProductsAndPricingNew.Application.Common.Errors;
using ProductsAndPricingNew.Application.Common.Mapping;
using ProductsAndPricingNew.Application.Features.Centre.Abstractions;
using ProductsAndPricingNew.Application.Features.Centre.Mappings;
using ProductsAndPricingNew.Domain.Common.Text;
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

        string name = request.Name.AsRequiredText(nameof(request.Name));

        bool isNameTaken = await _centreQuery.ExistsByNameAsync(name, request.Id, ct);
        if (isNameTaken)
            return Result.Fail(new ConflictError($"Centre name: '{name}' already exists"));

        var contactInfo = request.ContactInfo;
        var legalInfo = request.LegalInfo;
        var ratios = request.OperationalRatios;

        centre.Rename(name);
        centre.WithCode(request.Code);
        centre.WithCurrency(request.CurrencyId);
        centre.WithPrintFormat(request.PrintFormatId);
        centre.SetIsActive(request.IsActive);
        centre.SetIsPhysicalCentre(request.IsPhysicalCentre);
        centre.WithGeneralEmail(contactInfo.GeneralEmail);
        centre.WithAccommodationEmail(contactInfo.AccommodationEmail);
        centre.WithTelephone(contactInfo.Telephone);
        centre.WithEmergencyTelephone(contactInfo.EmergencyTelephone);
        centre.WithTransferEmergencyTelephone(contactInfo.TransferEmergencyTelephone);
        centre.WithBrandColor(contactInfo.BrandColor);
        centre.WithContactAddress(contactInfo.ContactAddress.ToDefinition());
        centre.WithLogo(contactInfo.LogoImage.ToDefinition());
        centre.WithSchoolSponsorshipNumber(legalInfo.SchoolSponsorshipNumber);
        centre.WithVatNumber(legalInfo.VatNumber);
        centre.WithRegistrationNumber(legalInfo.RegistrationNumber);
        centre.WithVatExemptionNumber(legalInfo.VatExemptionNumber);
        centre.WithChequePayableTo(legalInfo.ChequePayableTo);
        centre.WithGuarantees(ratios.Guarantees);
        centre.WithIndividualsRatio(ratios.IndividualsRatio);
        centre.WithStaffingRatio(ratios.StaffingRatio);
        centre.WithEmptyBeds(ratios.EmptyBeds);
        centre.WithBankDetails(request.BankDetails.ToDefinition());
        centre.WithContacts(request.Contacts.ToDefinitions());
        centre.WithTexts(request.Texts.ToDefinitions());

        await _unitOfWork.SaveChangesAsync(ct);

        return Result.Ok();
    }
}