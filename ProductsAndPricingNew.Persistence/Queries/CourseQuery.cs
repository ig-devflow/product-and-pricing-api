using System.Data.Common;
using Dapper;
using ProductsAndPricingNew.Application.Common.Pagination;
using ProductsAndPricingNew.Application.Features.Course.Abstractions;
using ProductsAndPricingNew.Application.Features.Course.Models;
using ProductsAndPricingNew.Persistence.Queries.Configuration;

namespace ProductsAndPricingNew.Persistence.Queries;

internal sealed class CourseQuery : BaseQuery, ICourseQuery
{
    private readonly ISqlConnectionFactory _connectionFactory;

    public CourseQuery(ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<bool> ExistsByNameAsync(string name, int? excludingId = null, CancellationToken ct = default)
    {
        const string sql = """
           SELECT CAST(CASE WHEN EXISTS (
               SELECT 1
               FROM Product.Course c
               WHERE c.IsDeleted = 0
                 AND c.Name = @Name
                 AND (@ExcludingId IS NULL OR c.Id <> @ExcludingId)
           ) THEN 1 ELSE 0 END AS bit);
           """;

        await using DbConnection connection = _connectionFactory.CreateConnection();

        CommandDefinition command = new(
            commandText: sql,
            parameters: new { Name = name, ExcludingId = excludingId },
            cancellationToken: ct);

        return await connection.QuerySingleAsync<bool>(command);
    }

    public async Task<CourseDetailsDto?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        const string sql = """
           SELECT
               c.Id,
               c.DivisionId,
               c.UnitTypeId,
               c.CourseLanguageId,
               c.CourseIntensityId,
               c.Name,
               c.IsActive,
               c.MinimumAge AS AgeFrom,
               c.MaximumAge AS AgeTo,
               c.MinimumWeeks,
               c.AccountCategoryId,
               c.ProductCategoryId,
               c.GeneralLedgerCode,
               c.CostCentreCode,
               c.OfferingsClosureDate AS ClosurePolicy,
               c.Version,
               c.CreatedAt,
               c.UpdatedAt,
               createdEditor.FirstName AS CreatedByFirstName,
               createdEditor.LastName  AS CreatedByLastName,
               updatedEditor.FirstName AS UpdatedByFirstName,
               updatedEditor.LastName  AS UpdatedByLastName
           FROM Product.Course c
           LEFT JOIN Edit.Editor createdEditor ON createdEditor.Id = c.CreatedById
           LEFT JOIN Edit.Editor updatedEditor ON updatedEditor.Id = c.UpdatedById
           WHERE c.Id = @Id
             AND c.IsDeleted = 0;
           """;

        await using DbConnection connection = _connectionFactory.CreateConnection();

        CommandDefinition command = new(
            commandText: sql,
            parameters: new { Id = id },
            cancellationToken: ct);

        CourseDetailsRow? row = await connection.QuerySingleOrDefaultAsync<CourseDetailsRow>(command);

        return row is null ? null : MapDetailsRow(row);
    }

    public async Task<PagedResult<CourseListItemDto>> GetListAsync(int divisionId, string? search, bool? isActive, PagingFilter paging, CancellationToken ct = default)
    {
        const string sql = """
           SELECT COUNT(1)
           FROM Product.Course c
           LEFT JOIN PricingRef.Division d ON d.Id = c.DivisionId AND d.IsDeleted = 0
           LEFT JOIN Edit.Editor createdEditor ON createdEditor.Id = c.CreatedById
           LEFT JOIN Edit.Editor updatedEditor ON updatedEditor.Id = c.UpdatedById
           WHERE c.IsDeleted = 0
             AND (@DivisionId = 0 OR c.DivisionId = @DivisionId)
             AND (@IsActive IS NULL OR c.IsActive = @IsActive)
             AND (
                 @Search IS NULL
                 OR c.Name LIKE '%' + @Search + '%'
                 OR d.Name LIKE '%' + @Search + '%'
                 OR createdEditor.FirstName LIKE '%' + @Search + '%'
                 OR createdEditor.LastName LIKE '%' + @Search + '%'
                 OR updatedEditor.FirstName LIKE '%' + @Search + '%'
                 OR updatedEditor.LastName LIKE '%' + @Search + '%'
             );

           SELECT
               c.Id,
               d.Name AS DivisionName,
               c.Name,
               c.IsActive,
               c.CourseLanguageId,
               c.CourseIntensityId,
               c.CreatedAt,
               c.UpdatedAt,
               createdEditor.FirstName AS CreatedByFirstName,
               createdEditor.LastName  AS CreatedByLastName,
               updatedEditor.FirstName AS UpdatedByFirstName,
               updatedEditor.LastName  AS UpdatedByLastName
           FROM Product.Course c
           LEFT JOIN PricingRef.Division d ON d.Id = c.DivisionId AND d.IsDeleted = 0
           LEFT JOIN Edit.Editor createdEditor ON createdEditor.Id = c.CreatedById
           LEFT JOIN Edit.Editor updatedEditor ON updatedEditor.Id = c.UpdatedById
           WHERE c.IsDeleted = 0
             AND (@DivisionId = 0 OR c.DivisionId = @DivisionId)
             AND (@IsActive IS NULL OR c.IsActive = @IsActive)
             AND (
                 @Search IS NULL
                 OR c.Name LIKE '%' + @Search + '%'
                 OR d.Name LIKE '%' + @Search + '%'
                 OR createdEditor.FirstName LIKE '%' + @Search + '%'
                 OR createdEditor.LastName LIKE '%' + @Search + '%'
                 OR updatedEditor.FirstName LIKE '%' + @Search + '%'
                 OR updatedEditor.LastName LIKE '%' + @Search + '%'
             )
           ORDER BY c.Name, c.Id
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

        IEnumerable<CourseListItemRow> rows = await grid.ReadAsync<CourseListItemRow>();
        List<CourseListItemDto> items = rows.Select(MapListItemRow).ToList();

        return new PagedResult<CourseListItemDto>(
            Items: items,
            TotalCount: totalCount,
            Page: page,
            PageSize: pageSize);
    }

    private static CourseDetailsDto MapDetailsRow(CourseDetailsRow row)
    {
        return new CourseDetailsDto(
            row.Id,
            row.DivisionId,
            row.UnitTypeId,
            row.CourseLanguageId,
            row.CourseIntensityId,
            row.Name,
            row.IsActive,
            row.AgeFrom,
            row.AgeTo,
            row.MinimumWeeks,
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

    private static CourseListItemDto MapListItemRow(CourseListItemRow row)
    {
        return new CourseListItemDto(
            row.Id,
            row.DivisionName,
            row.Name,
            row.IsActive,
            row.CourseLanguageId,
            row.CourseIntensityId,
            ToDateOnly(row.CreatedAt),
            BuildEditorName(row.CreatedByFirstName, row.CreatedByLastName),
            ToDateOnly(row.UpdatedAt),
            BuildEditorName(row.UpdatedByFirstName, row.UpdatedByLastName));
    }

    private sealed class CourseDetailsRow
    {
        public int Id { get; init; }
        public int DivisionId { get; init; }
        public int UnitTypeId { get; init; }
        public int CourseLanguageId { get; init; }
        public int CourseIntensityId { get; init; }
        public string Name { get; init; } = null!;
        public bool IsActive { get; init; }
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

    private sealed class CourseListItemRow
    {
        public int Id { get; init; }
        public string DivisionName { get; init; } = null!;
        public string Name { get; init; } = null!;
        public bool IsActive { get; init; }
        public int CourseLanguageId { get; init; }
        public int CourseIntensityId { get; init; }
        public DateTimeOffset CreatedAt { get; init; }
        public DateTimeOffset UpdatedAt { get; init; }
        public string CreatedByFirstName { get; init; } = null!;
        public string CreatedByLastName { get; init; } = null!;
        public string UpdatedByFirstName { get; init; } = null!;
        public string UpdatedByLastName { get; init; } = null!;
    }
}
