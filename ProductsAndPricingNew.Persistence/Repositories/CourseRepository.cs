using Microsoft.EntityFrameworkCore;
using ProductsAndPricingNew.Domain.Entities.Products;
using ProductsAndPricingNew.Domain.Repositories;

namespace ProductsAndPricingNew.Persistence.Repositories;

internal sealed class CourseRepository : EfRepositoryBase<Course, int>, ICourseRepository
{
    public CourseRepository(ProductsAndPricingDbContext db) : base(db) { }
}
