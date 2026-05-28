using System.Data.Common;
using Dapper;
using ProductsAndPricingNew.Application.Common.Pagination;
using ProductsAndPricingNew.Application.Features.Transfer.Abstractions;
using ProductsAndPricingNew.Application.Features.Transfer.Models;
using ProductsAndPricingNew.Persistence.Queries.Configuration;

namespace ProductsAndPricingNew.Persistence.Queries;

internal sealed class TransferQuery : BaseQuery, ITransferQuery
{
    private readonly ISqlConnectionFactory _connectionFactory;

    public TransferQuery(ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<bool> ExistsByNameAsync(string name, int? excludingId = null, CancellationToken ct = default)
    {
        const string sql = """
           SELECT CAST(CASE WHEN EXISTS (
               SELECT 1
               FROM Product.Transfer t
               WHERE t.IsDeleted = 0
                 AND t.Name = @Name
                 AND (@ExcludingId IS NULL OR t.Id <> @ExcludingId)
           ) THEN 1 ELSE 0 END AS bit);
           """;

        await using DbConnection connection = _connectionFactory.CreateConnection();

        CommandDefinition command = new(
            commandText: sql,
            parameters: new { Name = name, ExcludingId = excludingId },
            cancellationToken: ct);

        return await connection.QuerySingleAsync<bool>(command);
    }

    public async Task<TransferDetailsDto?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        const string sql = """
           SELECT
               t.Id,
               t.DivisionId,
               t.UnitTypeId,
               t.TransferTypeId,
               t.TransferPortId,
               t.TimeFrom,
               t.TimeTo,
               t.Name,
               t.IsActive,
               t.AccountCategoryId,
               t.ProductCategoryId,
               t.GeneralLedgerCode,
               t.CostCentreCode,
               t.OfferingsClosureDate AS ClosurePolicy,
               t.Version,
               t.CreatedAt,
               t.UpdatedAt,
               createdEditor.FirstName AS CreatedByFirstName,
               createdEditor.LastName  AS CreatedByLastName,
               updatedEditor.FirstName AS UpdatedByFirstName,
               updatedEditor.LastName  AS UpdatedByLastName
           FROM Product.Transfer t
           LEFT JOIN Edit.Editor createdEditor ON createdEditor.Id = t.CreatedById
           LEFT JOIN Edit.Editor updatedEditor ON updatedEditor.Id = t.UpdatedById
           WHERE t.Id = @Id
             AND t.IsDeleted = 0;
           """;

        await using DbConnection connection = _connectionFactory.CreateConnection();

        CommandDefinition command = new(
            commandText: sql,
            parameters: new { Id = id },
            cancellationToken: ct);

        TransferDetailsRow? row = await connection.QuerySingleOrDefaultAsync<TransferDetailsRow>(command);

        return row is null ? null : MapDetailsRow(row);
    }

    public async Task<PagedResult<TransferListItemDto>> GetListAsync(int divisionId, string? search, bool? isActive, PagingFilter paging, CancellationToken ct = default)
    {
        const string sql = """
           SELECT COUNT(1)
           FROM Product.Transfer t
           LEFT JOIN PricingRef.Division d ON d.Id = t.DivisionId AND d.IsDeleted = 0
           LEFT JOIN Edit.Editor createdEditor ON createdEditor.Id = t.CreatedById
           LEFT JOIN Edit.Editor updatedEditor ON updatedEditor.Id = t.UpdatedById
           WHERE t.IsDeleted = 0
             AND (@DivisionId = 0 OR t.DivisionId = @DivisionId)
             AND (@IsActive IS NULL OR t.IsActive = @IsActive)
             AND (
                 @Search IS NULL
                 OR t.Name LIKE '%' + @Search + '%'
                 OR d.Name LIKE '%' + @Search + '%'
                 OR createdEditor.FirstName LIKE '%' + @Search + '%'
                 OR createdEditor.LastName LIKE '%' + @Search + '%'
                 OR updatedEditor.FirstName LIKE '%' + @Search + '%'
                 OR updatedEditor.LastName LIKE '%' + @Search + '%'
             );

           SELECT
               t.Id,
               d.Name AS DivisionName,
               t.Name,
               t.IsActive,
               t.TransferTypeId,
               t.TransferPortId,
               t.CreatedAt,
               t.UpdatedAt,
               createdEditor.FirstName AS CreatedByFirstName,
               createdEditor.LastName  AS CreatedByLastName,
               updatedEditor.FirstName AS UpdatedByFirstName,
               updatedEditor.LastName  AS UpdatedByLastName
           FROM Product.Transfer t
           LEFT JOIN PricingRef.Division d ON d.Id = t.DivisionId AND d.IsDeleted = 0
           LEFT JOIN Edit.Editor createdEditor ON createdEditor.Id = t.CreatedById
           LEFT JOIN Edit.Editor updatedEditor ON updatedEditor.Id = t.UpdatedById
           WHERE t.IsDeleted = 0
             AND (@DivisionId = 0 OR t.DivisionId = @DivisionId)
             AND (@IsActive IS NULL OR t.IsActive = @IsActive)
             AND (
                 @Search IS NULL
                 OR t.Name LIKE '%' + @Search + '%'
                 OR d.Name LIKE '%' + @Search + '%'
                 OR createdEditor.FirstName LIKE '%' + @Search + '%'
                 OR createdEditor.LastName LIKE '%' + @Search + '%'
                 OR updatedEditor.FirstName LIKE '%' + @Search + '%'
                 OR updatedEditor.LastName LIKE '%' + @Search + '%'
             )
           ORDER BY t.Name, t.Id
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

        IEnumerable<TransferListItemRow> rows = await grid.ReadAsync<TransferListItemRow>();
        List<TransferListItemDto> items = rows.Select(MapListItemRow).ToList();

        return new PagedResult<TransferListItemDto>(
            Items: items,
            TotalCount: totalCount,
            Page: page,
            PageSize: pageSize);
    }

    private static TransferDetailsDto MapDetailsRow(TransferDetailsRow row)
    {
        return new TransferDetailsDto(
            row.Id,
            row.DivisionId,
            row.UnitTypeId,
            row.TransferTypeId,
            row.TransferPortId,
            row.TimeFrom,
            row.TimeTo,
            row.Name,
            row.IsActive,
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

    private static TransferListItemDto MapListItemRow(TransferListItemRow row)
    {
        return new TransferListItemDto(
            row.Id,
            row.DivisionName,
            row.Name,
            row.IsActive,
            row.TransferTypeId,
            row.TransferPortId,
            ToDateOnly(row.CreatedAt),
            BuildEditorName(row.CreatedByFirstName, row.CreatedByLastName),
            ToDateOnly(row.UpdatedAt),
            BuildEditorName(row.UpdatedByFirstName, row.UpdatedByLastName));
    }

    private sealed class TransferDetailsRow
    {
        public int Id { get; init; }
        public int DivisionId { get; init; }
        public int UnitTypeId { get; init; }
        public int TransferTypeId { get; init; }
        public int TransferPortId { get; init; }
        public TimeOnly? TimeFrom { get; init; }
        public TimeOnly? TimeTo { get; init; }
        public string Name { get; init; } = null!;
        public bool IsActive { get; init; }
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

    private sealed class TransferListItemRow
    {
        public int Id { get; init; }
        public string DivisionName { get; init; } = null!;
        public string Name { get; init; } = null!;
        public bool IsActive { get; init; }
        public int TransferTypeId { get; init; }
        public int TransferPortId { get; init; }
        public DateTimeOffset CreatedAt { get; init; }
        public DateTimeOffset UpdatedAt { get; init; }
        public string CreatedByFirstName { get; init; } = null!;
        public string CreatedByLastName { get; init; } = null!;
        public string UpdatedByFirstName { get; init; } = null!;
        public string UpdatedByLastName { get; init; } = null!;
    }
}
