using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Features.ReferenceData.Models;

namespace ProductsAndPricingNew.Application.Features.ReferenceData.Queries.GetCourseLanguages;

public sealed record GetCourseLanguagesQuery() : IRequest<Result<IReadOnlyCollection<CourseLanguageReferenceDto>>>;
