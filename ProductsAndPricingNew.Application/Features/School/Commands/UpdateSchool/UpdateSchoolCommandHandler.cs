using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Abstractions;
using ProductsAndPricingNew.Application.Common.Errors;
using ProductsAndPricingNew.Application.Common.Mapping;
using ProductsAndPricingNew.Application.Features.School.Abstractions;
using ProductsAndPricingNew.Domain.Common.Text;
using ProductsAndPricingNew.Domain.Repositories;
using SchoolEntity = ProductsAndPricingNew.Domain.Entities.PricingRef.School;

namespace ProductsAndPricingNew.Application.Features.School.Commands.UpdateSchool;

internal sealed class UpdateSchoolCommandHandler : IRequestHandler<UpdateSchoolCommand, Result<Unit>>
{
    private readonly ISchoolRepository _schoolRepository;
    private readonly ISchoolQuery _schoolQuery;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateSchoolCommandHandler(
        ISchoolRepository schoolRepository,
        ISchoolQuery schoolQuery,
        IUnitOfWork unitOfWork)
    {
        _schoolRepository = schoolRepository;
        _schoolQuery = schoolQuery;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Unit>> Handle(UpdateSchoolCommand request, CancellationToken ct)
    {
        SchoolEntity? school = await _schoolRepository.GetByIdAsync(request.Id, ct);
        if (school is null)
            return Result.Fail(new NotFoundError($"School with id {request.Id} was not found"));

        if (!school.HasVersion(request.Version))
            return Result.Fail(new ConflictError("School was modified by another user. Reload it and try again."));

        string name = request.Name.AsRequiredText(nameof(request.Name));

        bool isNameTaken = await _schoolQuery.ExistsByNameAsync(name, request.Id, ct);
        if (isNameTaken)
            return Result.Fail(new ConflictError($"School name: '{name}' already exists"));

        school.Rename(request.Name);
        school.WithCode(request.Code);
        school.WithMinimumStayInWeeks(request.MinimumStayInWeeks);
        school.WithAgeRange(request.AgeFrom, request.AgeTo);
        school.WithTelephone(request.Telephone);
        school.WithEmergencyTelephone(request.EmergencyTelephone);
        school.WithContactAddress(request.ContactAddress.ToDefinition());
        school.WithFinanceCode(request.FinanceCode);
        school.SetLmsAccess(request.LmsAccess);
        school.SetIsActive(request.IsActive);
        school.WithDecommissionDate(request.DecommissionDate);

        await _unitOfWork.SaveChangesAsync(ct);

        return Result.Ok();
    }
}