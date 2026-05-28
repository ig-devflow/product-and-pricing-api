using System.Data.Common;
using Dapper;
using ProductsAndPricingNew.Application.Common.Pagination;
using ProductsAndPricingNew.Application.Features.Package.Abstractions;
using ProductsAndPricingNew.Application.Features.Package.Models;
using ProductsAndPricingNew.Domain.Entities.Products;
using ProductsAndPricingNew.Persistence.Queries.Configuration;

namespace ProductsAndPricingNew.Persistence.Queries;

internal sealed class PackageQuery : BaseQuery, IPackageQuery
{
    private readonly ISqlConnectionFactory _connectionFactory;

    public PackageQuery(ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<bool> ExistsByNameAsync(string name, int? excludingId = null, CancellationToken ct = default)
    {
        const string sql = """
           SELECT CAST(CASE WHEN EXISTS (
               SELECT 1
               FROM Product.Package p
               WHERE p.IsDeleted = 0
                 AND p.Name = @Name
                 AND (@ExcludingId IS NULL OR p.Id <> @ExcludingId)
           ) THEN 1 ELSE 0 END AS bit);
           """;

        await using DbConnection connection = _connectionFactory.CreateConnection();

        CommandDefinition command = new(
            commandText: sql,
            parameters: new { Name = name, ExcludingId = excludingId },
            cancellationToken: ct);

        return await connection.QuerySingleAsync<bool>(command);
    }

    public async Task<PackageDetailsDto?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        const string sql = """
           SELECT
               p.Id,
               p.DivisionId,
               p.UnitTypeId,
               p.Name,
               p.IsActive,
               p.Description,
               p.CommissionPercentage AS Commission,
               p.MinimumAge AS AgeFrom,
               p.MaximumAge AS AgeTo,
               p.MinimumWeeks,
               p.AccountCategoryId,
               p.ProductCategoryId,
               p.GeneralLedgerCode,
               p.CostCentreCode,
               p.OfferingsClosureDate AS ClosurePolicy,
               p.Version,
               p.CreatedAt,
               p.UpdatedAt,
               createdEditor.FirstName AS CreatedByFirstName,
               createdEditor.LastName  AS CreatedByLastName,
               updatedEditor.FirstName AS UpdatedByFirstName,
               updatedEditor.LastName  AS UpdatedByLastName
           FROM Product.Package p
           LEFT JOIN Edit.Editor createdEditor ON createdEditor.Id = p.CreatedById
           LEFT JOIN Edit.Editor updatedEditor ON updatedEditor.Id = p.UpdatedById
           WHERE p.Id = @Id
             AND p.IsDeleted = 0;

           SELECT
               pi.ProductKind,
               pi.ProductDefinitionId AS ProductId,
               pi.PriceBreakdown
           FROM Product.PackageItem pi
           WHERE pi.PackageId = @Id;
           """;

        await using DbConnection connection = _connectionFactory.CreateConnection();

        CommandDefinition command = new(
            commandText: sql,
            parameters: new { Id = id },
            cancellationToken: ct);

        await using SqlMapper.GridReader grid = await connection.QueryMultipleAsync(command);

        PackageDetailsRow? row = await grid.ReadSingleOrDefaultAsync<PackageDetailsRow>();
        if (row is null)
            return null;

        IEnumerable<PackageItemRow> itemRows = await grid.ReadAsync<PackageItemRow>();
        List<PackageItemDto> items = itemRows
            .Select(r => new PackageItemDto((ProductKind)r.ProductKind, r.ProductId, r.PriceBreakdown))
            .ToList();

        return MapDetailsRow(row, items);
    }

    public async Task<PagedResult<PackageListItemDto>> GetListAsync(int divisionId, string? search, bool? isActive, PagingFilter paging, CancellationToken ct = default)
    {
        const string sql = """
           SELECT COUNT(1)
           FROM Product.Package p
           LEFT JOIN PricingRef.Division d ON d.Id = p.DivisionId AND d.IsDeleted = 0
           LEFT JOIN Edit.Editor createdEditor ON createdEditor.Id = p.CreatedById
           LEFT JOIN Edit.Editor updatedEditor ON updatedEditor.Id = p.UpdatedById
           WHERE p.IsDeleted = 0
             AND (@DivisionId = 0 OR p.DivisionId = @DivisionId)
             AND (@IsActive IS NULL OR p.IsActive = @IsActive)
             AND (
                 @Search IS NULL
                 OR p.Name LIKE '%' + @Search + '%'
                 OR d.Name LIKE '%' + @Search + '%'
                 OR createdEditor.FirstName LIKE '%' + @Search + '%'
                 OR createdEditor.LastName LIKE '%' + @Search + '%'
                 OR updatedEditor.FirstName LIKE '%' + @Search + '%'
                 OR updatedEditor.LastName LIKE '%' + @Search + '%'
             );

           SELECT
               p.Id,
               d.Name AS DivisionName,
               p.Name,
               p.IsActive,
               p.Description,
               p.CommissionPercentage AS Commission,
               p.CreatedAt,
               p.UpdatedAt,
               createdEditor.FirstName AS CreatedByFirstName,
               createdEditor.LastName  AS CreatedByLastName,
               updatedEditor.FirstName AS UpdatedByFirstName,
               updatedEditor.LastName  AS UpdatedByLastName
           FROM Product.Package p
           LEFT JOIN PricingRef.Division d ON d.Id = p.DivisionId AND d.IsDeleted = 0
           LEFT JOIN Edit.Editor createdEditor ON createdEditor.Id = p.CreatedById
           LEFT JOIN Edit.Editor updatedEditor ON updatedEditor.Id = p.UpdatedById
           WHERE p.IsDeleted = 0
             AND (@DivisionId = 0 OR p.DivisionId = @DivisionId)
             AND (@IsActive IS NULL OR p.IsActive = @IsActive)
             AND (
                 @Search IS NULL
                 OR p.Name LIKE '%' + @Search + '%'
                 OR d.Name LIKE '%' + @Search + '%'
                 OR createdEditor.FirstName LIKE '%' + @Search + '%'
                 OR createdEditor.LastName LIKE '%' + @Search + '%'
                 OR updatedEditor.FirstName LIKE '%' + @Search + '%'
                 OR updatedEditor.LastName LIKE '%' + @Search + '%'
             )
           ORDER BY p.Name, p.Id
           OFFSET @Offset ROWS
           FETCH NEXT @PageSize ROWS ONLY;
           """;

        int page = paging.GetPage();
        int pageSize = paging.GetPageSize();
        int offset = paging.GetOffset();

        var parameters = new
        {
            DivisionId = divisionId,
            Search = search,
            IsActive = isActive,
            Offset = offset,
            PageSize = pageSize
        };

        await using DbConnection connection = _connectionFactory.CreateConnection();

        CommandDefinition command = new(
            commandText: sql,
            parameters: parameters,
            cancellationToken: ct);

        await using SqlMapper.GridReader grid = await connection.QueryMultipleAsync(command);
        int totalCount = await grid.ReadSingleAsync<int>();

        IEnumerable<PackageListItemRow> rows = await grid.ReadAsync<PackageListItemRow>();
        List<PackageListItemDto> items = rows.Select(MapListItemRow).ToList();

        return new PagedResult<PackageListItemDto>(
            Items: items,
            TotalCount: totalCount,
            Page: page,
            PageSize: pageSize);
    }

    private static PackageDetailsDto MapDetailsRow(PackageDetailsRow row, IReadOnlyCollection<PackageItemDto> items)
    {
        return new PackageDetailsDto(
            row.Id,
            row.DivisionId,
            row.UnitTypeId,
            row.Name,
            row.IsActive,
            row.Description,
            row.Commission,
            row.AgeFrom,
            row.AgeTo,
            row.MinimumWeeks,
            row.AccountCategoryId,
            row.ProductCategoryId,
            row.GeneralLedgerCode,
            row.CostCentreCode,
            row.ClosurePolicy,
            items,
            ToBase64Version(row.Version),
            ToDateOnly(row.CreatedAt),
            BuildEditorName(row.CreatedByFirstName, row.CreatedByLastName),
            ToDateOnly(row.UpdatedAt),
            BuildEditorName(row.UpdatedByFirstName, row.UpdatedByLastName));
    }

    private static PackageListItemDto MapListItemRow(PackageListItemRow row)
    {
        return new PackageListItemDto(
            row.Id,
            row.DivisionName,
            row.Name,
            row.IsActive,
            row.Description,
            row.Commission,
            ToDateOnly(row.CreatedAt),
            BuildEditorName(row.CreatedByFirstName, row.CreatedByLastName),
            ToDateOnly(row.UpdatedAt),
            BuildEditorName(row.UpdatedByFirstName, row.UpdatedByLastName));
    }

    private sealed class PackageDetailsRow
    {
        public int Id { get; init; }
        public int DivisionId { get; init; }
        public int UnitTypeId { get; init; }
        public string Name { get; init; } = null!;
        public bool IsActive { get; init; }
        public string? Description { get; init; }
        public decimal Commission { get; init; }
        public int? AgeFrom { get; init; }
        public int? AgeTo { get; init; }
        public int? MinimumWeeks { get; init; }
        public int AccountCategoryId { get; init; }
        public int ProductCategoryId { get; init; }
        public string? GeneralLedgerCode { get; init; }
        public string? CostCentreCode { get; init; }
        public DateOnly? ClosurePolicy { get; init; }
        public byte[]? Version { get; init; }
        public DateTimeOffset CreatedAt { get; init; }
        public DateTimeOffset UpdatedAt { get; init; }
        public string CreatedByFirstName { get; init; } = null!;
        public string CreatedByLastName { get; init; } = null!;
        public string UpdatedByFirstName { get; init; } = null!;
        public string UpdatedByLastName { get; init; } = null!;
    }

    private sealed class PackageListItemRow
    {
        public int Id { get; init; }
        public string DivisionName { get; init; } = null!;
        public string Name { get; init; } = null!;
        public bool IsActive { get; init; }
        public string? Description { get; init; }
        public decimal Commission { get; init; }
        public DateTimeOffset CreatedAt { get; init; }
        public DateTimeOffset UpdatedAt { get; init; }
        public string CreatedByFirstName { get; init; } = null!;
        public string CreatedByLastName { get; init; } = null!;
        public string UpdatedByFirstName { get; init; } = null!;
        public string UpdatedByLastName { get; init; } = null!;
    }

    private sealed class PackageItemRow
    {
        public int ProductKind { get; init; }
        public int ProductId { get; init; }
        public decimal PriceBreakdown { get; init; }
    }
}
