using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Features.Course.Models;

namespace ProductsAndPricingNew.Application.Features.Course.Queries.GetCourseById;

public sealed record GetCourseByIdQuery(int Id) : IRequest<Result<CourseDetailsDto>>;
