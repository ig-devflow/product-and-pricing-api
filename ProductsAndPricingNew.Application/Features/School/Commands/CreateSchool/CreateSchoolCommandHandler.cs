using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Abstractions;
using ProductsAndPricingNew.Application.Features.School.Abstractions;
using ProductsAndPricingNew.Domain.Repositories;

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
        throw new NotImplementedException();
    }
}