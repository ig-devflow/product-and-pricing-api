using System.Data.Common;
using Dapper;
using ProductsAndPricingNew.Application.Common.Pagination;
using ProductsAndPricingNew.Application.Features.Accommodation.Abstractions;
using ProductsAndPricingNew.Application.Features.Accommodation.Models;
using ProductsAndPricingNew.Persistence.Queries.Configuration;

namespace ProductsAndPricingNew.Persistence.Queries;

internal sealed class AccommodationQuery : BaseQuery, IAccommodationQuery
{
    private readonly ISqlConnectionFactory _connectionFactory;

    public AccommodationQuery(ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<bool> ExistsByNameAsync(string name, int? excludingId = null, CancellationToken ct = default)
    {
        const string sql = """
           SELECT CAST(CASE WHEN EXISTS (
               SELECT 1
               FROM Product.Accommodation a
               WHERE a.IsDeleted = 0
                 AND a.Name = @Name
                 AND (@ExcludingId IS NULL OR a.Id <> @ExcludingId)
           ) THEN 1 ELSE 0 END AS bit);
           """;

        await using DbConnection connection = _connectionFactory.CreateConnection();

        CommandDefinition command = new(
            commandText: sql,
            parameters: new
            {
                Name = name,
                ExcludingId = excludingId
            },
            cancellationToken: ct);

        return await connection.QuerySingleAsync<bool>(command);
    }

    public async Task<AccommodationDetailsDto?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        const string sql = """
           SELECT
               a.Id,
               a.Name,
               a.AccommodationTypeId,
               a.IsActive,
               a.MinimumStayInWeeks,
               a.MinimumAge,
               a.MaximumAge,
               a.IsCommitted,
               a.IsNonCommitted,
               a.Version,
               a.CreatedAt,
               a.UpdatedAt,
               createdEditor.FirstName AS CreatedByFirstName,
               createdEditor.LastName  AS CreatedByLastName,
               updatedEditor.FirstName AS UpdatedByFirstName,
               updatedEditor.LastName  AS UpdatedByLastName
           FROM Product.Accommodation a
           LEFT JOIN Edit.Editor createdEditor
               ON createdEditor.Id = a.CreatedById
           LEFT JOIN Edit.Editor updatedEditor
               ON updatedEditor.Id = a.UpdatedById
           WHERE a.Id = @Id
             AND a.IsDeleted = 0;
           """;

        await using DbConnection connection = _connectionFactory.CreateConnection();

        CommandDefinition command = new(
            commandText: sql,
            parameters: new { Id = id },
            cancellationToken: ct);

        AccommodationDetailsRow? row = await connection.QuerySingleOrDefaultAsync<AccommodationDetailsRow>(command);

        return row is null ? null : MapDetailsRow(row);
    }

    public async Task<PagedResult<AccommodationListItemDto>> GetListAsync(string? search, bool? isActive, PagingFilter paging, CancellationToken ct = default)
    {
        const string sql = """
           SELECT COUNT(1)
           FROM Product.Accommodation a
           LEFT JOIN ReferenceData.AccommodationType acct
               ON acct.Id = a.AccommodationTypeId
              AND acct.IsDeleted = 0
           LEFT JOIN Edit.Editor createdEditor
               ON createdEditor.Id = a.CreatedById
           LEFT JOIN Edit.Editor updatedEditor
               ON updatedEditor.Id = a.UpdatedById
           WHERE a.IsDeleted = 0
             AND (@IsActive IS NULL OR a.IsActive = @IsActive)
             AND (
                 @Search IS NULL
                 OR a.Name LIKE '%' + @Search + '%'
                 OR acct.Name LIKE '%' + @Search + '%'
                 OR createdEditor.FirstName LIKE '%' + @Search + '%'
                 OR createdEditor.LastName LIKE '%' + @Search + '%'
                 OR updatedEditor.FirstName LIKE '%' + @Search + '%'
                 OR updatedEditor.LastName LIKE '%' + @Search + '%'
             );

           SELECT
               a.Id,
               a.Name,
               acct.Name AS AccommodationTypeName,
               a.IsActive,
               a.CreatedAt,
               a.UpdatedAt,
               createdEditor.FirstName AS CreatedByFirstName,
               createdEditor.LastName  AS CreatedByLastName,
               updatedEditor.FirstName AS UpdatedByFirstName,
               updatedEditor.LastName  AS UpdatedByLastName
           FROM Product.Accommodation a
           LEFT JOIN ReferenceData.AccommodationType acct
               ON acct.Id = a.AccommodationTypeId
              AND acct.IsDeleted = 0
           LEFT JOIN Edit.Editor createdEditor
               ON createdEditor.Id = a.CreatedById
           LEFT JOIN Edit.Editor updatedEditor
               ON updatedEditor.Id = a.UpdatedById
           WHERE a.IsDeleted = 0
             AND (@IsActive IS NULL OR a.IsActive = @IsActive)
             AND (
                 @Search IS NULL
                 OR a.Name LIKE '%' + @Search + '%'
                 OR acct.Name LIKE '%' + @Search + '%'
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

        IEnumerable<AccommodationListItemRow> rows = await grid.ReadAsync<AccommodationListItemRow>();
        List<AccommodationListItemDto> items = rows.Select(MapListItemRow).ToList();

        return new PagedResult<AccommodationListItemDto>(
            Items: items,
            TotalCount: totalCount,
            Page: page,
            PageSize: pageSize);
    }

    private static AccommodationDetailsDto MapDetailsRow(AccommodationDetailsRow row)
    {
        return new AccommodationDetailsDto(
            row.Id,
            row.Name,
            row.AccommodationTypeId,
            row.IsActive,
            row.MinimumStayInWeeks,
            row.MinimumAge,
            row.MaximumAge,
            row.IsCommitted,
            row.IsNonCommitted,
            ToBase64Version(row.Version),
            ToDateOnly(row.CreatedAt),
            BuildEditorName(row.CreatedByFirstName, row.CreatedByLastName),
            ToDateOnly(row.UpdatedAt),
            BuildEditorName(row.UpdatedByFirstName, row.UpdatedByLastName));
    }

    private static AccommodationListItemDto MapListItemRow(AccommodationListItemRow row)
    {
        return new AccommodationListItemDto(
            row.Id,
            row.Name,
            row.AccommodationTypeName,
            row.IsActive,
            ToDateOnly(row.CreatedAt),
            BuildEditorName(row.CreatedByFirstName, row.CreatedByLastName),
            ToDateOnly(row.UpdatedAt),
            BuildEditorName(row.UpdatedByFirstName, row.UpdatedByLastName));
    }

    private sealed class AccommodationDetailsRow
    {
        public int Id { get; init; }
        public string Name { get; init; } = null!;
        public int AccommodationTypeId { get; init; }
        public bool IsActive { get; init; }
        public int MinimumStayInWeeks { get; init; }
        public int? MinimumAge { get; init; }
        public int? MaximumAge { get; init; }
        public bool IsCommitted { get; init; }
        public bool IsNonCommitted { get; init; }
        public byte[]? Version { get; init; }
        public DateTimeOffset CreatedAt { get; init; }
        public DateTimeOffset UpdatedAt { get; init; }
        public string CreatedByFirstName { get; init; } = null!;
        public string CreatedByLastName { get; init; } = null!;
        public string UpdatedByFirstName { get; init; } = null!;
        public string UpdatedByLastName { get; init; } = null!;
    }

    private sealed class AccommodationListItemRow
    {
        public int Id { get; init; }
        public string Name { get; init; } = null!;
        public string AccommodationTypeName { get; init; } = null!;
        public bool IsActive { get; init; }
        public DateTimeOffset CreatedAt { get; init; }
        public DateTimeOffset UpdatedAt { get; init; }
        public string CreatedByFirstName { get; init; } = null!;
        public string CreatedByLastName { get; init; } = null!;
        public string UpdatedByFirstName { get; init; } = null!;
        public string UpdatedByLastName { get; init; } = null!;
    }
}