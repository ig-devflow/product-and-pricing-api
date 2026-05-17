using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Abstractions;
using ProductsAndPricingNew.Application.Common.Errors;
using ProductsAndPricingNew.Application.Common.Mapping;
using ProductsAndPricingNew.Application.Features.School.Abstractions;
using ProductsAndPricingNew.Domain.Common.Text;
using ProductsAndPricingNew.Domain.Repositories;
using SchoolEntity = ProductsAndPricingNew.Domain.Entities.PricingRef.School;

namespace ProductsAndPricingNew.Application.Features.School.Commands.CreateSchool;

internal sealed class CreateSchoolCommandHandler : IRequestHandler<CreateSchoolCommand, Result<int>>
{
    private readonly ISchoolRepository _schoolRepository;
    private readonly ISchoolQuery _schoolQuery;
    private readonly IUnitOfWork _unitOfWork;

    public CreateSchoolCommandHandler(
        ISchoolRepository schoolRepository,
        ISchoolQuery schoolQuery,
        IUnitOfWork unitOfWork)
    {
        _schoolRepository = schoolRepository;
        _schoolQuery = schoolQuery;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<int>> Handle(CreateSchoolCommand request, CancellationToken ct)
    {
        string name = request.Name.AsRequiredText(nameof(request.Name));

        bool isNameTaken = await _schoolQuery.ExistsByNameAsync(name, ct: ct);
        if (isNameTaken)
            return Result.Fail(new ConflictError($"School name: '{name}' already exists"));

        SchoolEntity school = new SchoolEntity.Builder(request.CentreId, name, request.LegacyCode)
            .MinimumStayInWeeks(request.MinimumStayInWeeks)
            .SetAgeRange(request.AgeFrom, request.AgeTo)
            .Telephone(request.Telephone)
            .EmergencyTelephone(request.EmergencyTelephone)
            .ContactAddress(request.ContactAddress.ToDefinition())
            .SetFinanceCode(request.FinanceCode)
            .LmsActive(request.LmsAccess)
            .IsActive(request.IsActive)
            .DecommissionDate(request.DecommissionDate)
            .Build();

        await _schoolRepository.AddAsync(school, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return Result.Ok(school.Id);
    }
}