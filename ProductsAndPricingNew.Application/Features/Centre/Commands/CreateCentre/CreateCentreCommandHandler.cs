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

namespace ProductsAndPricingNew.Application.Features.Centre.Commands.CreateCentre;

internal sealed class CreateCentreCommandHandler : IRequestHandler<CreateCentreCommand, Result<int>>
{
    private readonly ICentreRepository _centreRepository;
    private readonly ICentreQuery _centreQuery;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCentreCommandHandler(
        ICentreRepository centreRepository,
        ICentreQuery centreQuery,
        IUnitOfWork unitOfWork)
    {
        _centreRepository = centreRepository;
        _centreQuery = centreQuery;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<int>> Handle(CreateCentreCommand request, CancellationToken ct)
    {
        string name = request.Name.AsRequiredText(nameof(request.Name));

        bool isNameTaken = await _centreQuery.ExistsByNameAsync(name, ct: ct);
        if (isNameTaken)
            return Result.Fail(new ConflictError($"Centre name: '{name}' already exists"));

        var contactInfo = request.ContactInfo;
        var legalInfo = request.LegalInfo;
        var ratios = request.OperationalRatios;

        CentreEntity centre = new CentreEntity.Builder(name, request.Code, request.CurrencyId, request.PrintFormatId)
            .SetIsActive(request.IsActive)
            .SetIsPhysicalCentre(request.IsPhysicalCentre)
            .WithGeneralEmail(contactInfo.GeneralEmail)
            .WithAccommodationEmail(contactInfo.AccommodationEmail)
            .WithTelephone(contactInfo.Telephone)
            .WithEmergencyTelephone(contactInfo.EmergencyTelephone)
            .WithTransferEmergencyTelephone(contactInfo.TransferEmergencyTelephone)
            .WithBrandColor(contactInfo.BrandColor)
            .WithSchoolSponsorshipNumber(legalInfo.SchoolSponsorshipNumber)
            .WithVatNumber(legalInfo.VatNumber)
            .WithRegistrationNumber(legalInfo.RegistrationNumber)
            .WithVatExemptionNumber(legalInfo.VatExemptionNumber)
            .WithChequePayableTo(legalInfo.ChequePayableTo)
            .WithGuarantees(ratios.Guarantees)
            .WithIndividualsRatio(ratios.IndividualsRatio)
            .WithStaffingRatio(ratios.StaffingRatio)
            .WithEmptyBeds(ratios.EmptyBeds)
            .WithContactAddress(contactInfo.ContactAddress.ToDefinition())
            .WithLogoImage(contactInfo.LogoImage.ToDefinition())
            .WithBankDetails(request.BankDetails.ToDefinition())
            .WithContacts(request.Contacts.ToDefinitions())
            .WithTexts(request.Texts.ToDefinitions())
            .Build();

        await _centreRepository.AddAsync(centre, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return Result.Ok(centre.Id);
    }
}