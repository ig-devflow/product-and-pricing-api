using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Features.ReferenceData.Models;

namespace ProductsAndPricingNew.Application.Features.ReferenceData.Queries.GetCourseIntensities;

public sealed record GetCourseIntensitiesQuery() : IRequest<Result<IReadOnlyCollection<CourseIntensityReferenceDto>>>;
