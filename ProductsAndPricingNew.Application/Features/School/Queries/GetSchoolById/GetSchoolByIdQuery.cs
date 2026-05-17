using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Features.School.Models;

namespace ProductsAndPricingNew.Application.Features.School.Queries.GetSchoolById;

public sealed record GetSchoolByIdQuery(int Id) : IRequest<Result<SchoolDetailsDto>>;