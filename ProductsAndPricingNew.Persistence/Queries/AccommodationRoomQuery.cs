using System.Data.Common;
using Dapper;
using ProductsAndPricingNew.Application.Common.Pagination;
using ProductsAndPricingNew.Application.Features.AccommodationRoom.Abstractions;
using ProductsAndPricingNew.Application.Features.AccommodationRoom.Models;
using ProductsAndPricingNew.Persistence.Queries.Configuration;

namespace ProductsAndPricingNew.Persistence.Queries;

internal sealed class AccommodationRoomQuery : BaseQuery, IAccommodationRoomQuery
{
    private readonly ISqlConnectionFactory _connectionFactory;

    public AccommodationRoomQuery(ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<bool> ExistsByNameAsync(string name, int? excludingId = null, CancellationToken ct = default)
    {
        const string sql = """
           SELECT CAST(CASE WHEN EXISTS (
               SELECT 1
               FROM Product.AccommodationRoom ar
               WHERE ar.IsDeleted = 0
                 AND ar.Name = @Name
                 AND (@ExcludingId IS NULL OR ar.Id <> @ExcludingId)
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

    public async Task<AccommodationRoomDetailsDto?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        const string sql = """
           SELECT
               ar.Id,
               ar.AccommodationId,
               ar.DivisionId,
               ar.Name,
               ar.UnitTypeId,
               ar.IsActive,
               ar.OccupyRoom,
               ar.RoomTypeId,
               ar.BoardTypeId,
               ar.BathroomTypeId,
               ar.RoomGradeId,
               ar.AccountCategoryId,
               ar.ProductCategoryId,
               ar.GeneralLedgerCode,
               ar.CostCentreCode,
               ar.ProductCategoryId,
               ar.OfferingsClosureDate as ClosurePolicy,
               ar.Version,
               ar.CreatedAt,
               ar.UpdatedAt,
               createdEditor.FirstName AS CreatedByFirstName,
               createdEditor.LastName  AS CreatedByLastName,
               updatedEditor.FirstName AS UpdatedByFirstName,
               updatedEditor.LastName  AS UpdatedByLastName
           FROM Product.AccommodationRoom ar
           LEFT JOIN Edit.Editor createdEditor
               ON createdEditor.Id = ar.CreatedById
           LEFT JOIN Edit.Editor updatedEditor
               ON updatedEditor.Id = ar.UpdatedById
           WHERE ar.Id = @Id
             AND ar.IsDeleted = 0;
           """;

        await using DbConnection connection = _connectionFactory.CreateConnection();

        CommandDefinition command = new(
            commandText: sql,
            parameters: new { Id = id },
            cancellationToken: ct);

        AccommodationRoomDetailsRow? row = await connection.QuerySingleOrDefaultAsync<AccommodationRoomDetailsRow>(command);

        return row is null ? null : MapDetailsRow(row);
    }

    public async Task<PagedResult<AccommodationRoomListItemDto>> GetListAsync(int divisionId, int accommodationId, string? search, bool? isActive, PagingFilter paging, CancellationToken ct = default)
    {
        // todo: how to use int divisionId, int accommodationId
        const string sql = """
           SELECT COUNT(1)
           FROM Product.AccommodationRoom ar
           LEFT JOIN Product.Accommodation a
               ON a.Id = ar.AccommodationId
              AND a.IsDeleted = 0
           LEFT JOIN PricingRef.Division d
               ON d.Id = ar.DivisionId
              AND d.IsDeleted = 0
           LEFT JOIN Edit.Editor createdEditor
               ON createdEditor.Id = ar.CreatedById
           LEFT JOIN Edit.Editor updatedEditor
               ON updatedEditor.Id = ar.UpdatedById
           WHERE ar.IsDeleted = 0
             AND (@IsActive IS NULL OR a.IsActive = @IsActive)
             AND (
                 @Search IS NULL
                 OR ar.Name LIKE '%' + @Search + '%'
                 OR a.Name LIKE '%' + @Search + '%'
                 OR d.Name LIKE '%' + @Search + '%'
                 OR createdEditor.FirstName LIKE '%' + @Search + '%'
                 OR createdEditor.LastName LIKE '%' + @Search + '%'
                 OR updatedEditor.FirstName LIKE '%' + @Search + '%'
                 OR updatedEditor.LastName LIKE '%' + @Search + '%'
             );

           SELECT
               ar.Id,
               a.Name AS AccommodationName,
               d.Name AS DivisionName,
               ar.Name,
               ar.IsActive,
               ar.OccupyRoom,
               ar.CreatedAt,
               ar.UpdatedAt,
               createdEditor.FirstName AS CreatedByFirstName,
               createdEditor.LastName  AS CreatedByLastName,
               updatedEditor.FirstName AS UpdatedByFirstName,
               updatedEditor.LastName  AS UpdatedByLastName
           FROM Product.AccommodationRoom ar
           LEFT JOIN Product.Accommodation a
               ON a.Id = ar.AccommodationId
              AND a.IsDeleted = 0
           LEFT JOIN PricingRef.Division d
               ON d.Id = ar.DivisionId
              AND d.IsDeleted = 0
           LEFT JOIN Edit.Editor createdEditor
               ON createdEditor.Id = ar.CreatedById
           LEFT JOIN Edit.Editor updatedEditor
               ON updatedEditor.Id = ar.UpdatedById
           WHERE ar.IsDeleted = 0
             AND (@IsActive IS NULL OR a.IsActive = @IsActive)
             AND (
                 @Search IS NULL
                 OR ar.Name LIKE '%' + @Search + '%'
                 OR a.Name LIKE '%' + @Search + '%'
                 OR d.Name LIKE '%' + @Search + '%'
                 OR createdEditor.FirstName LIKE '%' + @Search + '%'
                 OR createdEditor.LastName LIKE '%' + @Search + '%'
                 OR updatedEditor.FirstName LIKE '%' + @Search + '%'
                 OR updatedEditor.LastName LIKE '%' + @Search + '%'
             )
           ORDER BY ar.Name, ar.Id
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

        IEnumerable<AccommodationRoomListItemRow> rows = await grid.ReadAsync<AccommodationRoomListItemRow>();
        List<AccommodationRoomListItemDto> items = rows.Select(MapListItemRow).ToList();

        return new PagedResult<AccommodationRoomListItemDto>(
            Items: items,
            TotalCount: totalCount,
            Page: page,
            PageSize: pageSize);
    }

    private static AccommodationRoomDetailsDto MapDetailsRow(AccommodationRoomDetailsRow row)
    {
        return new AccommodationRoomDetailsDto(
            row.Id,
            row.AccommodationId,
            row.DivisionId,
            row.UnitTypeId,
            row.Name,
            row.IsActive,
            row.OccupyRoom,
            BuildRoomDetails(row.RoomTypeId, row.BoardTypeId, row.BathroomTypeId, row.RoomGradeId),
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

    private static AccommodationRoomListItemDto MapListItemRow(AccommodationRoomListItemRow row)
    {
        return new AccommodationRoomListItemDto(
            row.Id,
            row.AccommodationName,
            row.DivisionName,
            row.Name,
            row.IsActive,
            row.OccupyRoom,
            ToDateOnly(row.CreatedAt),
            BuildEditorName(row.CreatedByFirstName, row.CreatedByLastName),
            ToDateOnly(row.UpdatedAt),
            BuildEditorName(row.UpdatedByFirstName, row.UpdatedByLastName));
    }

    private static RoomDetailsDto BuildRoomDetails(int roomTypeId, int boardTypeId, int bathroomTypeId, int roomGradeId) =>
        new(roomTypeId, boardTypeId, bathroomTypeId, roomGradeId);

    private sealed class AccommodationRoomDetailsRow
    {
        public int Id { get; init; }
        public int AccommodationId { get; init; }
        public int DivisionId { get; init; }
        public int UnitTypeId { get; init; }
        public string Name { get; init; } = null!;
        public bool IsActive { get; init; }
        public bool OccupyRoom { get; init; }
        public int RoomTypeId { get; init; }
        public int BoardTypeId { get; init; }
        public int BathroomTypeId { get; init; }
        public int RoomGradeId { get; init; }
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

    private sealed class AccommodationRoomListItemRow
    {
        public int Id { get; init; }
        public string AccommodationName { get; init; } = null!;
        public string DivisionName { get; init; } = null!;
        public string Name { get; init; } = null!;
        public bool IsActive { get; init; }
        public bool OccupyRoom { get; init; }
        public DateTimeOffset CreatedAt { get; init; }
        public DateTimeOffset UpdatedAt { get; init; }
        public string CreatedByFirstName { get; init; } = null!;
        public string CreatedByLastName { get; init; } = null!;
        public string UpdatedByFirstName { get; init; } = null!;
        public string UpdatedByLastName { get; init; } = null!;
    }
}