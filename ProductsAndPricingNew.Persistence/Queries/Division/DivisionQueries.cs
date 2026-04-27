using System.Data.Common;
using Dapper;
using ProductsAndPricingNew.Application.Common.Models;
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
               FROM PricingRef.Division d
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
                d.Street,
                d.District,
                d.City,
                d.PostalCode,
                d.AddressCountryId,
                d.AccreditationBannerData,
                d.AccreditationBannerContentType,
                d.AccreditationBannerFileName
            FROM PricingRef.Division d
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
            MapBanner(row),
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
           FROM PricingRef.Division d
           WHERE d.IsDeleted = 0
             AND (@IsActive IS NULL OR d.IsActive = @IsActive)
             AND (@Search IS NULL OR d.Name LIKE '%' + @Search + '%');

           SELECT
               d.Id,
               d.Name,
               d.IsActive
           FROM PricingRef.Division d
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

    private static ImageBannerDto? MapBanner(DivisionDetailsRow row)
    {
        bool isEmpty =
            row.AccreditationBannerData is null &&
            row.AccreditationBannerContentType is null &&
            row.AccreditationBannerFileName is null;

        if (isEmpty)
            return null;

        return new ImageBannerDto(
            row.AccreditationBannerData,
            row.AccreditationBannerContentType,
            row.AccreditationBannerFileName);
    }

    private static AddressDto? MapAddress(DivisionDetailsRow row)
    {
        bool isEmpty =
            row.Street is null &&
            row.District is null &&
            row.City is null &&
            row.PostalCode is null &&
            row.CountryId is null;

        if (isEmpty)
            return null;

        return new AddressDto(
            row.Street,
            row.District,
            row.City,
            row.PostalCode,
            row.CountryId);
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
        public string? Street { get; init; }
        public string? District { get; init; }
        public string? City { get; init; }
        public string? PostalCode { get; init; }
        public int? CountryId { get; init; }
        public byte[]? AccreditationBannerData { get; init; }
        public string? AccreditationBannerContentType { get; init; }
        public string? AccreditationBannerFileName { get; init; }
    }

    private sealed class DivisionListItemRow
    {
        public int Id { get; init; }
        public string Name { get; init; } = null!;
        public bool IsActive { get; init; }
    }
}