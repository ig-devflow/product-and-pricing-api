using System.Data.Common;
using Dapper;
using ProductsAndPricingNew.Application.Common.Models;
using ProductsAndPricingNew.Application.Common.Pagination;
using ProductsAndPricingNew.Application.Features.School.Abstractions;
using ProductsAndPricingNew.Application.Features.School.Models;
using ProductsAndPricingNew.Persistence.Queries.Configuration;

namespace ProductsAndPricingNew.Persistence.Queries;

internal sealed class SchoolQuery : ISchoolQuery
{
    private readonly ISqlConnectionFactory _connectionFactory;

    public SchoolQuery(ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<bool> ExistsByNameAsync(string name, int? excludingId = null, CancellationToken ct = default)
    {
        const string sql = """
           SELECT CAST(CASE WHEN EXISTS (
               SELECT 1
               FROM PricingRef.School s
               WHERE s.IsDeleted = 0
                 AND s.Name = @Name
                 AND (@ExcludingId IS NULL OR s.Id <> @ExcludingId)
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

    public async Task<SchoolDetailsDto?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        const string sql = """
            SELECT
                s.Id,
                s.CentreId,
                s.Name,
                s.LegacyCode,
                s.MinimumStayInWeeks,
                s.AgeFrom,
                s.AgeTo,
                s.Telephone,
                s.EmergencyTelephone,
                s.ContactStreet,
                s.ContactDistrict,
                s.ContactCity,
                s.ContactPostalCode,
                s.ContactCountryId,
                s.FinanceCode,
                s.LmsAccess,
                s.IsActive,
                s.DecommissionDate,
                s.Version,
                s.CreatedAt,
                s.UpdatedAt,
                createdEditor.FirstName AS CreatedByFirstName,
                createdEditor.LastName  AS CreatedByLastName,
                updatedEditor.FirstName AS UpdatedByFirstName,
                updatedEditor.LastName  AS UpdatedByLastName
            FROM PricingRef.School s
            LEFT JOIN Edit.Editor createdEditor
                ON createdEditor.Id = s.CreatedById
            LEFT JOIN Edit.Editor updatedEditor
                ON updatedEditor.Id = s.UpdatedById
            WHERE s.Id = @Id
              AND s.IsDeleted = 0;
            """;

        await using DbConnection connection = _connectionFactory.CreateConnection();

        CommandDefinition command = new(
            commandText: sql,
            parameters: new { Id = id },
            cancellationToken: ct);

        SchoolDetailsRow? row = await connection.QuerySingleOrDefaultAsync<SchoolDetailsRow>(command);

        return row is null ? null : MapDetailsRow(row);
    }

    public async Task<PagedResult<SchoolListItemDto>> GetListAsync(
        string? search,
        bool? isActive,
        PagingFilter paging,
        CancellationToken ct = default)
    {
        const string sql = """
           SELECT COUNT(1)
           FROM PricingRef.School s
           INNER JOIN PricingRef.Centre centre
               ON centre.Id = s.CentreId
              AND centre.IsDeleted = 0
           LEFT JOIN ReferenceData.Country country
               ON country.Id = s.ContactCountryId
              AND country.IsDeleted = 0
           LEFT JOIN Edit.Editor createdEditor
               ON createdEditor.Id = s.CreatedById
           LEFT JOIN Edit.Editor updatedEditor
               ON updatedEditor.Id = s.UpdatedById
           WHERE s.IsDeleted = 0
             AND (@IsActive IS NULL OR s.IsActive = @IsActive)
             AND (
                 @Search IS NULL
                 OR s.Name LIKE '%' + @Search + '%'
                 OR s.LegacyCode LIKE '%' + @Search + '%'
                 OR centre.Name LIKE '%' + @Search + '%'
                 OR s.ContactCity LIKE '%' + @Search + '%'
                 OR country.Name LIKE '%' + @Search + '%'
                 OR createdEditor.FirstName LIKE '%' + @Search + '%'
                 OR createdEditor.LastName LIKE '%' + @Search + '%'
                 OR updatedEditor.FirstName LIKE '%' + @Search + '%'
                 OR updatedEditor.LastName LIKE '%' + @Search + '%'
             );

           SELECT
               s.Id,
               s.Name,
               s.LegacyCode,
               centre.Name AS CentreName,
               s.IsActive,
               s.ContactCity AS City,
               country.Name AS CountryName,
               s.DecommissionDate,
               s.CreatedAt,
               s.UpdatedAt,
               createdEditor.FirstName AS CreatedByFirstName,
               createdEditor.LastName  AS CreatedByLastName,
               updatedEditor.FirstName AS UpdatedByFirstName,
               updatedEditor.LastName  AS UpdatedByLastName
           FROM PricingRef.School s
           INNER JOIN PricingRef.Centre centre
               ON centre.Id = s.CentreId
              AND centre.IsDeleted = 0
           LEFT JOIN ReferenceData.Country country
               ON country.Id = s.ContactCountryId
              AND country.IsDeleted = 0
           LEFT JOIN Edit.Editor createdEditor
               ON createdEditor.Id = s.CreatedById
           LEFT JOIN Edit.Editor updatedEditor
               ON updatedEditor.Id = s.UpdatedById
           WHERE s.IsDeleted = 0
             AND (@IsActive IS NULL OR s.IsActive = @IsActive)
             AND (
                 @Search IS NULL
                 OR s.Name LIKE '%' + @Search + '%'
                 OR s.LegacyCode LIKE '%' + @Search + '%'
                 OR centre.Name LIKE '%' + @Search + '%'
                 OR s.ContactCity LIKE '%' + @Search + '%'
                 OR country.Name LIKE '%' + @Search + '%'
                 OR createdEditor.FirstName LIKE '%' + @Search + '%'
                 OR createdEditor.LastName LIKE '%' + @Search + '%'
                 OR updatedEditor.FirstName LIKE '%' + @Search + '%'
                 OR updatedEditor.LastName LIKE '%' + @Search + '%'
             )
           ORDER BY s.Name, s.Id
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

        IEnumerable<SchoolListItemRow> rows = await grid.ReadAsync<SchoolListItemRow>();
        List<SchoolListItemDto> items = rows.Select(MapListItemRow).ToList();

        return new PagedResult<SchoolListItemDto>(
            Items: items,
            TotalCount: totalCount,
            Page: page,
            PageSize: pageSize);
    }

    private static SchoolDetailsDto MapDetailsRow(SchoolDetailsRow row)
    {
        return new SchoolDetailsDto(
            row.Id,
            row.CentreId,
            row.Name,
            row.LegacyCode,
            row.MinimumStayInWeeks,
            row.AgeFrom,
            row.AgeTo,
            row.Telephone,
            row.EmergencyTelephone,
            MapAddress(row.ContactStreet, row.ContactDistrict, row.ContactCity, row.ContactPostalCode, row.ContactCountryId),
            row.FinanceCode,
            row.LmsAccess,
            row.IsActive,
            row.DecommissionDate,
            ToBase64Version(row.Version),
            ToDateOnly(row.CreatedAt),
            BuildEditorName(row.CreatedByFirstName, row.CreatedByLastName),
            ToDateOnly(row.UpdatedAt),
            BuildEditorName(row.UpdatedByFirstName, row.UpdatedByLastName));
    }

    private static SchoolListItemDto MapListItemRow(SchoolListItemRow row)
    {
        return new SchoolListItemDto(
            row.Id,
            row.Name,
            row.LegacyCode,
            row.CentreName,
            row.IsActive,
            row.City,
            row.CountryName,
            row.DecommissionDate,
            ToDateOnly(row.CreatedAt),
            BuildEditorName(row.CreatedByFirstName, row.CreatedByLastName),
            ToDateOnly(row.UpdatedAt),
            BuildEditorName(row.UpdatedByFirstName, row.UpdatedByLastName));
    }

    private static AddressDto? MapAddress(string? street, string? district, string? city, string? postalCode, int? countryId)
    {
        bool isEmpty = street is null && district is null && city is null && postalCode is null && countryId is null;
        return isEmpty ? null : new AddressDto(street, district, city, postalCode, countryId);
    }

    internal static string? BuildEditorName(string? firstName, string? lastName)
    {
        string name = string.Join(" ", new[] { firstName, lastName }.Where(v => !string.IsNullOrWhiteSpace(v)));
        return string.IsNullOrWhiteSpace(name) ? null : name;
    }

    private static DateOnly ToDateOnly(DateTimeOffset value) => DateOnly.FromDateTime(value.DateTime);

    internal static string ToBase64Version(byte[]? version) => Convert.ToBase64String(version ?? []);

    private sealed class SchoolDetailsRow
    {
        public int Id { get; init; }
        public int CentreId { get; init; }
        public string Name { get; init; } = null!;
        public string LegacyCode { get; init; } = null!;
        public int MinimumStayInWeeks { get; init; }
        public int? AgeFrom { get; init; }
        public int? AgeTo { get; init; }
        public string? Telephone { get; init; }
        public string? EmergencyTelephone { get; init; }
        public string? ContactStreet { get; init; }
        public string? ContactDistrict { get; init; }
        public string? ContactCity { get; init; }
        public string? ContactPostalCode { get; init; }
        public int? ContactCountryId { get; init; }
        public string FinanceCode { get; init; } = null!;
        public bool LmsAccess { get; init; }
        public bool IsActive { get; init; }
        public DateOnly? DecommissionDate { get; init; }
        public byte[]? Version { get; init; }
        public DateTimeOffset CreatedAt { get; init; }
        public DateTimeOffset UpdatedAt { get; init; }
        public string? CreatedByFirstName { get; init; }
        public string? CreatedByLastName { get; init; }
        public string? UpdatedByFirstName { get; init; }
        public string? UpdatedByLastName { get; init; }
    }

    private sealed class SchoolListItemRow
    {
        public int Id { get; init; }
        public string Name { get; init; } = null!;
        public string LegacyCode { get; init; } = null!;
        public string CentreName { get; init; } = null!;
        public bool IsActive { get; init; }
        public string? City { get; init; }
        public string? CountryName { get; init; }
        public DateOnly? DecommissionDate { get; init; }
        public DateTimeOffset CreatedAt { get; init; }
        public DateTimeOffset UpdatedAt { get; init; }
        public string? CreatedByFirstName { get; init; }
        public string? CreatedByLastName { get; init; }
        public string? UpdatedByFirstName { get; init; }
        public string? UpdatedByLastName { get; init; }
    }
}