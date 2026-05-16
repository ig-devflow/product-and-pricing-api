namespace ProductsAndPricingNew.Application.Features.School.Abstractions;

public interface ISchoolQuery
{
    Task<bool> ExistsByNameAsync(string name, int? excludingId = null, CancellationToken ct = default);
}