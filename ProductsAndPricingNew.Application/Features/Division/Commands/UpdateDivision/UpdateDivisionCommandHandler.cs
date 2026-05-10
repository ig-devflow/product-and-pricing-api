using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Abstractions;
using ProductsAndPricingNew.Application.Common.Errors;
using ProductsAndPricingNew.Application.Features.Division.Abstractions;
using ProductsAndPricingNew.Domain.Abstractions;
using ProductsAndPricingNew.Domain.Common.Text;
using ProductsAndPricingNew.Domain.SharedKernel.TextContent;
using ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;
using DivisionEntity = ProductsAndPricingNew.Domain.Entities.PricingRef.Division;

namespace ProductsAndPricingNew.Application.Features.Division.Commands.UpdateDivision;

internal sealed class UpdateDivisionCommandHandler : IRequestHandler<UpdateDivisionCommand, Result<Unit>>
{
    private readonly IDivisionRepository _divisionRepository;
    private readonly IDivisionQuery _divisionQuery;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateDivisionCommandHandler(
        IDivisionRepository divisionRepository,
        IDivisionQuery divisionQuery,
        IUnitOfWork unitOfWork)
    {
        _divisionRepository = divisionRepository;
        _divisionQuery = divisionQuery;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Unit>> Handle(UpdateDivisionCommand request, CancellationToken ct)
    {
        DivisionEntity? division = await _divisionRepository.GetByIdWithTextsAsync(request.Id, ct);
        if (division is null)
            return Result.Fail(new NotFoundError($"Division with id {request.Id} was not found"));

        if (!division.HasVersion(request.Version))
            return Result.Fail(new ConflictError("Division was modified by another user. Reload it and try again."));

        string name = request.Name.AsRequiredText(nameof(request.Name));

        bool isNameTaken = await _divisionQuery.ExistsByNameAsync(name, request.Id, ct);
        if (isNameTaken)
            return Result.Fail(new ConflictError($"Division name: '{name}' already exists"));

        var address = request.ContactAddress;
        var banner = request.AccreditationBanner;

        division.Rename(name);
        division.ChangeActiveState(request.IsActive);
        division.ChangeTermsAndConditions(request.TermsAndConditions);
        division.ChangeGroupsPaymentTerms(request.GroupsPaymentTerms);
        division.ChangeWebsite(request.WebsiteUrl);
        division.ChangeHeadOfficeEmail(request.HeadOfficeEmail);
        division.ChangeHeadOfficeTelephone(request.HeadOfficeTelephoneNo);
        division.ChangeContactAddress(address?.CountryId, address?.Street, address?.District, address?.City, address?.PostalCode);
        division.ChangeAccreditationBanner(banner?.Data, banner?.ContentType, banner?.FileName);
        division.ReplaceTexts(request.Texts.Select(x => new TextContentDefinition(x.ContentTemplateId, x.AudienceId, x.Content, x.Format)));

        await _unitOfWork.SaveChangesAsync(ct);

        return Result.Ok(Unit.Value);
    }
}