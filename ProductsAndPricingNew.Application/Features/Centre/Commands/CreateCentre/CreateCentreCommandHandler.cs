using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Abstractions;
using ProductsAndPricingNew.Application.Common.Errors;
using ProductsAndPricingNew.Application.Common.Mapping;
using ProductsAndPricingNew.Application.Features.Centre.Abstractions;
using ProductsAndPricingNew.Application.Features.Centre.Mappings;
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
        bool isNameTaken = await _centreQuery.ExistsByNameAsync(request.Name, ct: ct);
        if (isNameTaken)
            return Result.Fail(new ConflictError($"Centre name: '{request.Name}' already exists"));

        var contactInfo = request.ContactInfo;
        var legalInfo = request.LegalInfo;
        var ratios = request.OperationalRatios;

        CentreEntity centre = new CentreEntity.Builder(request.Name, request.Code, request.CurrencyId, request.PrintFormat)
            .IsActive(request.IsActive)
            .IsPhysicalCentre(request.IsPhysicalCentre)
            .GeneralEmail(contactInfo.GeneralEmail)
            .AccommodationEmail(contactInfo.AccommodationEmail)
            .Telephone(contactInfo.Telephone)
            .EmergencyTelephone(contactInfo.EmergencyTelephone)
            .TransferEmergencyTelephone(contactInfo.TransferEmergencyTelephone)
            .BrandColor(contactInfo.BrandColor)
            .SchoolSponsorshipNumber(legalInfo.SchoolSponsorshipNumber)
            .VatNumber(legalInfo.VatNumber)
            .RegistrationNumber(legalInfo.RegistrationNumber)
            .VatExemptionNumber(legalInfo.VatExemptionNumber)
            .ChequePayableTo(legalInfo.ChequePayableTo)
            .Guarantees(ratios.Guarantees)
            .IndividualsRatio(ratios.IndividualsRatio)
            .StaffingRatio(ratios.StaffingRatio)
            .EmptyBeds(ratios.EmptyBeds)
            .ContactAddress(contactInfo.ContactAddress.ToDefinition())
            .LogoImage(contactInfo.LogoImage.ToDefinition())
            .BankDetails(request.BankDetails.ToDefinition())
            .Contacts(request.Contacts.ToDefinitions())
            .Texts(request.Texts.ToDefinitions())
            .Build();

        await _centreRepository.AddAsync(centre, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return Result.Ok(centre.Id);
    }
}