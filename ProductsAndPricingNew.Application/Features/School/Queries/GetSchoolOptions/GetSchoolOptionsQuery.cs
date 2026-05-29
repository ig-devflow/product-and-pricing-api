using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Features.School.Models;

namespace ProductsAndPricingNew.Application.Features.School.Queries.GetSchoolOptions;

public sealed record GetSchoolOptionsQuery(int? CentreId = null) : IRequest<Result<IReadOnlyCollection<SchoolOptionDto>>>;
