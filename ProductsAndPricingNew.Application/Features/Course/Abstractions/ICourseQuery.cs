namespace ProductsAndPricingNew.Application.Features.Course.Abstractions;

public interface ICourseQuery
{
    Task<bool> ExistsByNameAsync(string name, int? excludingId = null, CancellationToken ct = default);
}