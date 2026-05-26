using System.Data.Common;
using Dapper;
using ProductsAndPricingNew.Application.Common.Pagination;
using ProductsAndPricingNew.Application.Features.Division.Abstractions;
using ProductsAndPricingNew.Application.Features.Division.Models;
using ProductsAndPricingNew.Persistence.Queries.Configuration;

namespace ProductsAndPricingNew.Persistence.Queries;

internal sealed class DivisionQuery : BaseQuery, IDivisionQuery
{
    private readonly ISqlConnectionFactory _connectionFactory;

    public DivisionQuery(ISqlConnectionFactory connectionFactory)
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
                d.ContactStreet,
                d.ContactDistrict,
                d.ContactCity,
                d.ContactPostalCode,
                d.ContactCountryId,
                d.BannerData AS AccreditationBannerData,
                d.BannerContentType AS AccreditationBannerContentType,
                d.BannerFileName AS AccreditationBannerFileName,
                d.Version,
                d.CreatedAt,
                d.UpdatedAt,
                createdEditor.FirstName AS CreatedByFirstName,
                createdEditor.LastName AS CreatedByLastName,
                updatedEditor.FirstName AS UpdatedByFirstName,
                updatedEditor.LastName AS UpdatedByLastName
            FROM PricingRef.Division d
            LEFT JOIN Edit.Editor createdEditor
                ON createdEditor.Id = d.CreatedById
            LEFT JOIN Edit.Editor updatedEditor
                ON updatedEditor.Id = d.UpdatedById
            WHERE d.Id = @Id
              AND d.IsDeleted = 0;

            SELECT
                dtc.Id,
                dtc.ContentTemplateId,
                ct.Name AS ContentTemplateName,
                dtc.AudienceId,
                a.Name AS AudienceName,
                dtc.Content,
                dtc.Format
            FROM PricingRef.DivisionTextContent dtc
            INNER JOIN ReferenceData.ContentTemplate ct
                ON ct.Id = dtc.ContentTemplateId
            LEFT JOIN ReferenceData.Audience a
                ON a.Id = dtc.AudienceId
            WHERE dtc.DivisionId = @Id
              AND dtc.IsDeleted = 0
            ORDER BY ct.Name, a.Name;
            """;

        await using DbConnection connection = _connectionFactory.CreateConnection();

        CommandDefinition command = new(
            commandText: sql,
            parameters: new { Id = id },
            cancellationToken: ct);

        await using SqlMapper.GridReader grid = await connection.QueryMultipleAsync(command);

        DivisionDetailsRow? row = await grid.ReadSingleOrDefaultAsync<DivisionDetailsRow>();

        if (row is null)
            return null;

        List<DivisionTextContentDto> texts = (await grid.ReadAsync<DivisionTextContentDto>()).AsList();

        return MapDetailsRow(row, texts);
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
           LEFT JOIN ReferenceData.Country c
               ON c.Id = d.ContactCountryId
              AND c.IsDeleted = 0
           LEFT JOIN Edit.Editor createdEditor
               ON createdEditor.Id = d.CreatedById
           LEFT JOIN Edit.Editor updatedEditor
               ON updatedEditor.Id = d.UpdatedById
           WHERE d.IsDeleted = 0
             AND (@IsActive IS NULL OR d.IsActive = @IsActive)
             AND (
                 @Search IS NULL
                 OR d.Name LIKE '%' + @Search + '%'
                 OR d.WebsiteUrl LIKE '%' + @Search + '%'
                 OR d.HeadOfficeEmail LIKE '%' + @Search + '%'
                 OR d.ContactCity LIKE '%' + @Search + '%'
                 OR c.Name LIKE '%' + @Search + '%'
                 OR createdEditor.FirstName LIKE '%' + @Search + '%'
                 OR createdEditor.LastName LIKE '%' + @Search + '%'
                 OR updatedEditor.FirstName LIKE '%' + @Search + '%'
                 OR updatedEditor.LastName LIKE '%' + @Search + '%'
             );

           SELECT
               d.Id,
               d.Name,
               d.IsActive,
               d.WebsiteUrl,
               d.HeadOfficeEmail,
               d.ContactCity,
               c.Name AS CountryName,
               d.CreatedAt,
               d.UpdatedAt,
               createdEditor.FirstName AS CreatedByFirstName,
               createdEditor.LastName AS CreatedByLastName,
               updatedEditor.FirstName AS UpdatedByFirstName,
               updatedEditor.LastName AS UpdatedByLastName
           FROM PricingRef.Division d
           LEFT JOIN ReferenceData.Country c
               ON c.Id = d.ContactCountryId
              AND c.IsDeleted = 0
           LEFT JOIN Edit.Editor createdEditor
               ON createdEditor.Id = d.CreatedById
           LEFT JOIN Edit.Editor updatedEditor
               ON updatedEditor.Id = d.UpdatedById
           WHERE d.IsDeleted = 0
             AND (@IsActive IS NULL OR d.IsActive = @IsActive)
             AND (
                 @Search IS NULL
                 OR d.Name LIKE '%' + @Search + '%'
                 OR d.WebsiteUrl LIKE '%' + @Search + '%'
                 OR d.HeadOfficeEmail LIKE '%' + @Search + '%'
                 OR d.ContactCity LIKE '%' + @Search + '%'
                 OR c.Name LIKE '%' + @Search + '%'
                 OR createdEditor.FirstName LIKE '%' + @Search + '%'
                 OR createdEditor.LastName LIKE '%' + @Search + '%'
                 OR updatedEditor.FirstName LIKE '%' + @Search + '%'
                 OR updatedEditor.LastName LIKE '%' + @Search + '%'
             )
           ORDER BY d.Name, d.Id
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

        IEnumerable<DivisionListItemRow> rows = await grid.ReadAsync<DivisionListItemRow>();
        List<DivisionListItemDto> items = rows.Select(MapListItemRow).ToList();

        return new PagedResult<DivisionListItemDto>(
            Items: items,
            TotalCount: totalCount,
            Page: page,
            PageSize: pageSize);
    }

    internal static DivisionDetailsDto MapDetailsRow(DivisionDetailsRow row, IReadOnlyCollection<DivisionTextContentDto> texts)
    {
        return new DivisionDetailsDto(
            row.Id,
            row.Name,
            row.IsActive,
            row.TermsAndConditions,
            row.GroupsPaymentTerms,
            row.WebsiteUrl,
            row.HeadOfficeEmail,
            row.HeadOfficeTelephoneNo,
            MapImage(row.AccreditationBannerData, row.AccreditationBannerContentType, row.AccreditationBannerFileName),
            MapAddress(row.Street, row.District, row.City, row.PostalCode, row.CountryId),
            texts,
            ToBase64Version(row.Version),
            ToDateOnly(row.CreatedAt),
            BuildEditorName(row.CreatedByFirstName, row.CreatedByLastName),
            ToDateOnly(row.UpdatedAt),
            BuildEditorName(row.UpdatedByFirstName, row.UpdatedByLastName));
    }

    internal static DivisionListItemDto MapListItemRow(DivisionListItemRow row)
    {
        return new DivisionListItemDto(
            row.Id,
            row.Name,
            row.IsActive,
            row.WebsiteUrl,
            row.HeadOfficeEmail,
            row.City,
            row.CountryName,
            ToDateOnly(row.CreatedAt),
            BuildEditorName(row.CreatedByFirstName, row.CreatedByLastName),
            ToDateOnly(row.UpdatedAt),
            BuildEditorName(row.UpdatedByFirstName, row.UpdatedByLastName));
    }

    internal sealed class DivisionDetailsRow
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
        public byte[]? Version { get; init; }
        public DateTimeOffset CreatedAt { get; init; }
        public DateTimeOffset UpdatedAt { get; init; }
        public string CreatedByFirstName { get; init; } = null!;
        public string CreatedByLastName { get; init; } = null!;
        public string UpdatedByFirstName { get; init; } = null!;
        public string UpdatedByLastName { get; init; } = null!;
    }

    internal sealed class DivisionListItemRow
    {
        public int Id { get; init; }
        public string Name { get; init; } = null!;
        public bool IsActive { get; init; }
        public string? WebsiteUrl { get; init; }
        public string? HeadOfficeEmail { get; init; }
        public string? City { get; init; }
        public string? CountryName { get; init; }
        public DateTimeOffset CreatedAt { get; init; }
        public DateTimeOffset UpdatedAt { get; init; }
        public string CreatedByFirstName { get; init; } = null!;
        public string CreatedByLastName { get; init; } = null!;
        public string UpdatedByFirstName { get; init; } = null!;
        public string UpdatedByLastName { get; init; } = null!;
    }
}
