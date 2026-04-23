using System.Data.Common;
using Dapper;
using ProductsAndPricingNew.Application.Common.Pagination;
using ProductsAndPricingNew.Application.Features.Division.Abstractions;
using ProductsAndPricingNew.Application.Features.Division.Models;
using ProductsAndPricingNew.Persistence.Queries.Configuration;

namespace ProductsAndPricingNew.Persistence.Queries.Division;

internal sealed class DivisionQueries : IDivisionQueries
{
    private readonly ISqlConnectionFactory _connectionFactory;

    public DivisionQueries(ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<bool> ExistsByNameAsync(string name, int? excludingId = null, CancellationToken ct = default)
    {
        const string sql = """
           SELECT CAST(CASE WHEN EXISTS (
               SELECT 1
               FROM dbo.Division d
               WHERE d.IsDeleted = 0
                 AND d.Name = @Name
                 AND (@ExcludingId IS NULL OR d.Id <> @ExcludingId)
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

    public async Task<DivisionDetailsDto?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        const string sql = """
            SELECT
                d.Id,
                d.Name,
                d.IsActive,
                d.TermsAndConditions,
                d.GroupsPaymentTerms,
                d.WebsiteUrl,
                d.HeadOfficeEmail,
                d.HeadOfficeTelephoneNo,
                d.AddressLine1,
                d.AddressLine2,
                d.AddressLine3,
                d.AddressLine4,
                d.AddressCountryId
            FROM dbo.Division d
            WHERE d.Id = @Id
              AND d.IsDeleted = 0;
            """;

        await using DbConnection connection = _connectionFactory.CreateConnection();

        CommandDefinition command = new(
            commandText: sql,
            parameters: new { Id = id },
            cancellationToken: ct);

        DivisionDetailsRow? row = await connection.QuerySingleOrDefaultAsync<DivisionDetailsRow>(command);

        if (row is null)
            return null;

        return new DivisionDetailsDto(
            row.Id,
            row.Name,
            row.IsActive,
            row.TermsAndConditions,
            row.GroupsPaymentTerms,
            row.WebsiteUrl,
            row.HeadOfficeEmail,
            row.HeadOfficeTelephoneNo,
            MapAddress(row));
    }

    public async Task<PagedResult<DivisionListItemDto>> GetListAsync(
        string? search,
        bool? isActive,
        PagingFilter paging,
        CancellationToken ct = default)
    {
        const string sql = """
           SELECT COUNT(1)
           FROM dbo.Division d
           WHERE d.IsDeleted = 0
             AND (@IsActive IS NULL OR d.IsActive = @IsActive)
             AND (@Search IS NULL OR d.Name LIKE '%' + @Search + '%');

           SELECT
               d.Id,
               d.Name,
               d.IsActive
           FROM dbo.Division d
           WHERE d.IsDeleted = 0
             AND (@IsActive IS NULL OR d.IsActive = @IsActive)
             AND (@Search IS NULL OR d.Name LIKE '%' + @Search + '%')
           ORDER BY d.Name, d.Id
           OFFSET @Offset ROWS
           FETCH NEXT @PageSize ROWS ONLY;
           """;

        int page = paging.GetPage();
        int pageSize = paging.GetPageSize();
        int offset = (page - 1) * pageSize;

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

        IEnumerable<DivisionListItemRow> rows = await grid.ReadAsync<DivisionListItemRow>();
        List<DivisionListItemDto> items = new();

        foreach (DivisionListItemRow row in rows)
        {
            items.Add(new DivisionListItemDto(row.Id, row.Name, row.IsActive));
        }

        return new PagedResult<DivisionListItemDto>(
            Items: items,
            TotalCount: totalCount,
            Page: page,
            PageSize: pageSize);
    }

    private static DivisionAddressDto? MapAddress(DivisionDetailsRow row)
    {
        bool isEmpty =
            row.AddressLine1 is null &&
            row.AddressLine2 is null &&
            row.AddressLine3 is null &&
            row.AddressLine4 is null &&
            row.AddressCountryId is null;

        if (isEmpty)
            return null;

        return new DivisionAddressDto(
            row.AddressLine1,
            row.AddressLine2,
            row.AddressLine3,
            row.AddressLine4,
            row.AddressCountryId);
    }

    private sealed class DivisionDetailsRow
    {
        public int Id { get; init; }
        public string Name { get; init; } = null!;
        public bool IsActive { get; init; }
        public string? TermsAndConditions { get; init; }
        public string? GroupsPaymentTerms { get; init; }
        public string? WebsiteUrl { get; init; }
        public string? HeadOfficeEmail { get; init; }
        public string? HeadOfficeTelephoneNo { get; init; }
        public string? AddressLine1 { get; init; }
        public string? AddressLine2 { get; init; }
        public string? AddressLine3 { get; init; }
        public string? AddressLine4 { get; init; }
        public int? AddressCountryId { get; init; }
    }

    private sealed class DivisionListItemRow
    {
        public int Id { get; init; }
        public string Name { get; init; } = null!;
        public bool IsActive { get; init; }
    }
}