using System.Data.Common;
using Dapper;
using ProductsAndPricingNew.Application.Common.Pagination;
using ProductsAndPricingNew.Application.Features.AddOn.Abstractions;
using ProductsAndPricingNew.Application.Features.AddOn.Models;
using ProductsAndPricingNew.Domain.Entities.Products;
using ProductsAndPricingNew.Persistence.Queries.Configuration;

namespace ProductsAndPricingNew.Persistence.Queries;

internal sealed class AddOnQuery : BaseQuery, IAddOnQuery
{
    private readonly ISqlConnectionFactory _connectionFactory;

    public AddOnQuery(ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<bool> ExistsByNameAsync(string name, int? excludingId = null, CancellationToken ct = default)
    {
        const string sql = """
           SELECT CAST(CASE WHEN EXISTS (
               SELECT 1
               FROM Product.AddOn a
               WHERE a.IsDeleted = 0
                 AND a.Name = @Name
                 AND (@ExcludingId IS NULL OR a.Id <> @ExcludingId)
           ) THEN 1 ELSE 0 END AS bit);
           """;

        await using DbConnection connection = _connectionFactory.CreateConnection();

        CommandDefinition command = new(
            commandText: sql,
            parameters: new { Name = name, ExcludingId = excludingId },
            cancellationToken: ct);

        return await connection.QuerySingleAsync<bool>(command);
    }

    public async Task<AddOnDetailsDto?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        const string sql = """
           SELECT
               a.Id,
               a.DivisionId,
               a.UnitTypeId,
               a.AddOnTypeId,
               a.Name,
               a.IsActive,
               a.MinimumAge AS AgeFrom,
               a.MaximumAge AS AgeTo,
               a.OneToOneLessonsPerWeek,
               a.AccountCategoryId,
               a.ProductCategoryId,
               a.GeneralLedgerCode,
               a.CostCentreCode,
               a.OfferingsClosureDate AS ClosurePolicy,
               a.Version,
               a.CreatedAt,
               a.UpdatedAt,
               createdEditor.FirstName AS CreatedByFirstName,
               createdEditor.LastName  AS CreatedByLastName,
               updatedEditor.FirstName AS UpdatedByFirstName,
               updatedEditor.LastName  AS UpdatedByLastName
           FROM Product.AddOn a
           LEFT JOIN Edit.Editor createdEditor ON createdEditor.Id = a.CreatedById
           LEFT JOIN Edit.Editor updatedEditor ON updatedEditor.Id = a.UpdatedById
           WHERE a.Id = @Id
             AND a.IsDeleted = 0;
           """;

        await using DbConnection connection = _connectionFactory.CreateConnection();

        CommandDefinition command = new(
            commandText: sql,
            parameters: new { Id = id },
            cancellationToken: ct);

        AddOnDetailsRow? row = await connection.QuerySingleOrDefaultAsync<AddOnDetailsRow>(command);

        return row is null ? null : MapDetailsRow(row);
    }

    public async Task<PagedResult<AddOnListItemDto>> GetListAsync(int divisionId, string? search, bool? isActive, PagingFilter paging, CancellationToken ct = default)
    {
        const string sql = """
           SELECT COUNT(1)
           FROM Product.AddOn a
           LEFT JOIN PricingRef.Division d ON d.Id = a.DivisionId AND d.IsDeleted = 0
           LEFT JOIN Edit.Editor createdEditor ON createdEditor.Id = a.CreatedById
           LEFT JOIN Edit.Editor updatedEditor ON updatedEditor.Id = a.UpdatedById
           WHERE a.IsDeleted = 0
             AND (@DivisionId = 0 OR a.DivisionId = @DivisionId)
             AND (@IsActive IS NULL OR a.IsActive = @IsActive)
             AND (
                 @Search IS NULL
                 OR a.Name LIKE '%' + @Search + '%'
                 OR d.Name LIKE '%' + @Search + '%'
                 OR createdEditor.FirstName LIKE '%' + @Search + '%'
                 OR createdEditor.LastName LIKE '%' + @Search + '%'
                 OR updatedEditor.FirstName LIKE '%' + @Search + '%'
                 OR updatedEditor.LastName LIKE '%' + @Search + '%'
             );

           SELECT
               a.Id,
               d.Name AS DivisionName,
               a.Name,
               a.IsActive,
               a.AddOnTypeId,
               a.CreatedAt,
               a.UpdatedAt,
               createdEditor.FirstName AS CreatedByFirstName,
               createdEditor.LastName  AS CreatedByLastName,
               updatedEditor.FirstName AS UpdatedByFirstName,
               updatedEditor.LastName  AS UpdatedByLastName
           FROM Product.AddOn a
           LEFT JOIN PricingRef.Division d ON d.Id = a.DivisionId AND d.IsDeleted = 0
           LEFT JOIN Edit.Editor createdEditor ON createdEditor.Id = a.CreatedById
           LEFT JOIN Edit.Editor updatedEditor ON updatedEditor.Id = a.UpdatedById
           WHERE a.IsDeleted = 0
             AND (@DivisionId = 0 OR a.DivisionId = @DivisionId)
             AND (@IsActive IS NULL OR a.IsActive = @IsActive)
             AND (
                 @Search IS NULL
                 OR a.Name LIKE '%' + @Search + '%'
                 OR d.Name LIKE '%' + @Search + '%'
                 OR createdEditor.FirstName LIKE '%' + @Search + '%'
                 OR createdEditor.LastName LIKE '%' + @Search + '%'
                 OR updatedEditor.FirstName LIKE '%' + @Search + '%'
                 OR updatedEditor.LastName LIKE '%' + @Search + '%'
             )
           ORDER BY a.Name, a.Id
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

        IEnumerable<AddOnListItemRow> rows = await grid.ReadAsync<AddOnListItemRow>();
        List<AddOnListItemDto> items = rows.Select(MapListItemRow).ToList();

        return new PagedResult<AddOnListItemDto>(
            Items: items,
            TotalCount: totalCount,
            Page: page,
            PageSize: pageSize);
    }

    private static AddOnDetailsDto MapDetailsRow(AddOnDetailsRow row)
    {
        return new AddOnDetailsDto(
            row.Id,
            row.DivisionId,
            row.UnitTypeId,
            (AddOnType)row.AddOnTypeId,
            row.Name,
            row.IsActive,
            row.AgeFrom,
            row.AgeTo,
            row.OneToOneLessonsPerWeek,
            row.AccountCategoryId,
            row.ProductCategoryId,
            row.GeneralLedgerCode,
            row.CostCentreCode,
            row.ClosurePolicy,
            ToBase64Version(row.Version),
            ToDateOnly(row.CreatedAt),
            BuildEditorName(row.CreatedByFirstName, row.CreatedByLastName),
            ToDateOnly(row.UpdatedAt),
            BuildEditorName(row.UpdatedByFirstName, row.UpdatedByLastName));
    }

    private static AddOnListItemDto MapListItemRow(AddOnListItemRow row)
    {
        return new AddOnListItemDto(
            row.Id,
            row.DivisionName,
            row.Name,
            row.IsActive,
            (AddOnType)row.AddOnTypeId,
            ToDateOnly(row.CreatedAt),
            BuildEditorName(row.CreatedByFirstName, row.CreatedByLastName),
            ToDateOnly(row.UpdatedAt),
            BuildEditorName(row.UpdatedByFirstName, row.UpdatedByLastName));
    }

    private sealed class AddOnDetailsRow
    {
        public int Id { get; init; }
        public int DivisionId { get; init; }
        public int UnitTypeId { get; init; }
        public int AddOnTypeId { get; init; }
        public string Name { get; init; } = null!;
        public bool IsActive { get; init; }
        public int? AgeFrom { get; init; }
        public int? AgeTo { get; init; }
        public int? OneToOneLessonsPerWeek { get; init; }
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

    private sealed class AddOnListItemRow
    {
        public int Id { get; init; }
        public string DivisionName { get; init; } = null!;
        public string Name { get; init; } = null!;
        public bool IsActive { get; init; }
        public int AddOnTypeId { get; init; }
        public DateTimeOffset CreatedAt { get; init; }
        public DateTimeOffset UpdatedAt { get; init; }
        public string CreatedByFirstName { get; init; } = null!;
        public string CreatedByLastName { get; init; } = null!;
        public string UpdatedByFirstName { get; init; } = null!;
        public string UpdatedByLastName { get; init; } = null!;
    }
}
