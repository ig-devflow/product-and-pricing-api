using System.Data.Common;
using Dapper;
using ProductsAndPricingNew.Application.Features.ProductCategory.Abstractions;
using ProductsAndPricingNew.Application.Features.ProductCategory.Models;
using ProductsAndPricingNew.Persistence.Queries.Configuration;

namespace ProductsAndPricingNew.Persistence.Queries;

internal sealed class ProductCategoryQuery : BaseQuery, IProductCategoryQuery
{
    private readonly ISqlConnectionFactory _connectionFactory;

    public ProductCategoryQuery(ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<bool> ExistsByNameAsync(string name, int divisionId, int? excludingId = null, CancellationToken ct = default)
    {
        const string sql = """
            SELECT CAST(CASE WHEN EXISTS (
                SELECT 1
                FROM Product.ProductCategory c
                WHERE c.IsDeleted = 0
                  AND c.DivisionId = @DivisionId
                  AND c.Name = @Name
                  AND (@ExcludingId IS NULL OR c.Id <> @ExcludingId)
            ) THEN 1 ELSE 0 END AS bit);
            """;

        await using DbConnection connection = _connectionFactory.CreateConnection();

        CommandDefinition command = new(
            commandText: sql,
            parameters: new { Name = name, DivisionId = divisionId, ExcludingId = excludingId },
            cancellationToken: ct);

        return await connection.QuerySingleAsync<bool>(command);
    }

    public async Task<ProductCategoryDetailsDto?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        const string sql = """
            SELECT
                c.Id,
                c.DivisionId,
                c.Name,
                c.IsActive,
                c.Version,
                c.CreatedAt,
                c.UpdatedAt,
                createdEditor.FirstName AS CreatedByFirstName,
                createdEditor.LastName  AS CreatedByLastName,
                updatedEditor.FirstName AS UpdatedByFirstName,
                updatedEditor.LastName  AS UpdatedByLastName
            FROM Product.ProductCategory c
            LEFT JOIN Edit.Editor createdEditor
                ON createdEditor.Id = c.CreatedById
            LEFT JOIN Edit.Editor updatedEditor
                ON updatedEditor.Id = c.UpdatedById
            WHERE c.Id = @Id
              AND c.IsDeleted = 0;
            """;

        await using DbConnection connection = _connectionFactory.CreateConnection();

        CommandDefinition command = new(
            commandText: sql,
            parameters: new { Id = id },
            cancellationToken: ct);

        ProductCategoryDetailsRow? row = await connection.QuerySingleOrDefaultAsync<ProductCategoryDetailsRow>(command);

        return row is null ? null : MapDetailsRow(row);
    }

    public async Task<IReadOnlyCollection<ProductCategoryListItemDto>> GetListByDivisionAsync(int divisionId, CancellationToken ct = default)
    {
        const string sql = """
            SELECT c.Id, c.Name
            FROM Product.ProductCategory c
            WHERE c.IsDeleted = 0
              AND c.DivisionId = @DivisionId
            ORDER BY c.Name;
            """;

        await using DbConnection connection = _connectionFactory.CreateConnection();

        CommandDefinition command = new(
            commandText: sql,
            parameters: new { DivisionId = divisionId },
            cancellationToken: ct);

        return (await connection.QueryAsync<ProductCategoryListItemDto>(command)).AsList();
    }

    private ProductCategoryDetailsDto MapDetailsRow(ProductCategoryDetailsRow row)
    {
        return new ProductCategoryDetailsDto(
            row.Id,
            row.DivisionId,
            row.Name,
            row.IsActive,
            ToBase64Version(row.Version),
            ToDateOnly(row.CreatedAt),
            BuildEditorName(row.CreatedByFirstName, row.CreatedByLastName),
            ToDateOnly(row.UpdatedAt),
            BuildEditorName(row.UpdatedByFirstName, row.UpdatedByLastName));
    }

    private sealed class ProductCategoryDetailsRow
    {
        public int Id { get; init; }
        public int DivisionId { get; init; }
        public string Name { get; init; } = null!;
        public bool IsActive { get; init; }
        public byte[]? Version { get; init; }
        public DateTimeOffset CreatedAt { get; init; }
        public DateTimeOffset UpdatedAt { get; init; }
        public string CreatedByFirstName { get; init; } = null!;
        public string CreatedByLastName { get; init; } = null!;
        public string UpdatedByFirstName { get; init; } = null!;
        public string UpdatedByLastName { get; init; } = null!;
    }
}
