using AutoMapper;
using ProductsAndPricingNew.Application.Common.Pagination;
using ProductsAndPricingNew.Application.Features.Course.Commands.CreateCourse;
using ProductsAndPricingNew.Application.Features.Course.Commands.UpdateCourse;
using ProductsAndPricingNew.Application.Features.Course.Queries.GetCourses;

namespace ProductsAndPricingNew.AdminApi.Contracts.Course;

public class CourseMappingProfile : Profile
{
    public CourseMappingProfile()
    {
        CreateMap<GetCoursesRequest, GetCoursesQuery>()
            .ConstructUsing(src => new GetCoursesQuery(
                0,
                src.Search,
                src.IsActive,
                new PagingFilter(src.Page, src.PageSize)));

        CreateMap<CreateCourseRequest, CreateCourseCommand>()
            .ConstructUsing(src => new CreateCourseCommand(
                0,
                src.UnitTypeId,
                src.CourseLanguageId,
                src.CourseIntensityId,
                src.Name,
                src.IsActive,
                src.ProductCategoryId,
                src.AccountCategoryId,
                src.AgeFrom,
                src.AgeTo,
                src.MinimumWeeks,
                src.GeneralLedgerCode,
                src.CostCentreCode,
                src.ClosurePolicy));

        CreateMap<UpdateCourseRequest, UpdateCourseCommand>()
            .ConstructUsing(src => new UpdateCourseCommand(
                0,
                src.UnitTypeId,
                src.CourseLanguageId,
                src.CourseIntensityId,
                src.Name,
                src.IsActive,
                src.ProductCategoryId,
                src.AccountCategoryId,
                src.AgeFrom,
                src.AgeTo,
                src.MinimumWeeks,
                src.GeneralLedgerCode,
                src.CostCentreCode,
                src.ClosurePolicy,
                src.Version));
    }
}
