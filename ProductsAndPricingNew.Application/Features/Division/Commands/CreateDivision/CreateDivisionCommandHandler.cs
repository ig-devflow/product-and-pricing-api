using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Abstractions;
using ProductsAndPricingNew.Application.Common.Errors;
using ProductsAndPricingNew.Application.Common.Models;
using ProductsAndPricingNew.Application.Features.Division.Abstractions;
using ProductsAndPricingNew.Domain.Abstractions;
using ProductsAndPricingNew.Domain.Common.Text;
using ProductsAndPricingNew.Domain.ReferenceData;
using DivisionEntity = ProductsAndPricingNew.Domain.Entities.PricingRef.Division;

namespace ProductsAndPricingNew.Application.Features.Division.Commands.CreateDivision;

internal sealed class CreateDivisionCommandHandler : IRequestHandler<CreateDivisionCommand, Result<int>>
{
    private readonly IDivisionRepository _divisionRepository;
    private readonly IDivisionQuery _divisionQuery;
    private readonly IUnitOfWork _unitOfWork;

    public CreateDivisionCommandHandler(
        IDivisionRepository divisionRepository,
        IDivisionQuery divisionQuery,
        IUnitOfWork unitOfWork)
    {
        _divisionRepository = divisionRepository;
        _divisionQuery = divisionQuery;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<int>> Handle(CreateDivisionCommand request, CancellationToken ct)
    {
        string name = request.Name.AsRequiredText();

        bool isNameTaken = await _divisionQuery.ExistsByNameAsync(name, ct: ct);
        if (isNameTaken)
            return Result.Fail(new ConflictError($"Division name: '{name}' already exists"));

        AddressDto? address = request.ContactAddress;
        ImageBannerDto? banner = request.AccreditationBanner;

        DivisionEntity division = new DivisionEntity.Builder(name, request.WebsiteUrl)
            .IsActive(request.IsActive)
            .TermsAndConditions(request.TermsAndConditions)
            .GroupsPaymentTerms(request.GroupsPaymentTerms)
            .HeadOfficeEmail(request.HeadOfficeEmail)
            .HeadOfficeTelephone(request.HeadOfficeTelephoneNo)
            .ContactAddress(address?.CountryId, address?.Street, address?.District, address?.City, address?.PostalCode)
            .AccreditationBanner(banner?.Data, banner?.ContentType, banner?.FileName)
            .Texts(request.Texts.Select(x => new TextContentDefinition(x.ContentTemplateId, x.AudienceId, x.Content, x.Format)))
            .Build();

        await _divisionRepository.AddAsync(division, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return Result.Ok(division.Id);
    }
}
