using System.Data.Common;
using Dapper;
using ProductsAndPricingNew.Application.Common.Models;
using ProductsAndPricingNew.Application.Common.Pagination;
using ProductsAndPricingNew.Application.Features.Centre.Abstractions;
using ProductsAndPricingNew.Application.Features.Centre.Models;
using ProductsAndPricingNew.Domain.Entities.PricingRef;
using ProductsAndPricingNew.Persistence.Queries.Configuration;

namespace ProductsAndPricingNew.Persistence.Queries.Centre;

internal sealed class CentreQuery : ICentreQuery
{
    private readonly ISqlConnectionFactory _connectionFactory;

    public CentreQuery(ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<bool> ExistsByNameAsync(string name, int? excludingId = null, CancellationToken ct = default)
    {
        const string sql = """
           SELECT CAST(CASE WHEN EXISTS (
               SELECT 1
               FROM PricingRef.Centre c
               WHERE c.IsDeleted = 0
                 AND c.Name = @Name
                 AND (@ExcludingId IS NULL OR c.Id <> @ExcludingId)
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

    public async Task<CentreDetailsDto?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        const string sql = """
            SELECT
                c.Id,
                c.Name,
                c.Code,
                c.CurrencyId,
                c.PrintFormat,
                c.IsActive,
                c.IsPhysicalCentre,
                -- ContactInfo
                c.GeneralEmail,
                c.AccommodationEmail,
                c.Telephone,
                c.EmergencyTelephone,
                c.TransferEmergencyTelephone,
                c.BrandColor,
                c.ContactStreet,
                c.ContactDistrict,
                c.ContactCity,
                c.ContactPostalCode,
                c.ContactCountryId,
                c.LogoData,
                c.LogoContentType,
                c.LogoFileName,
                -- LegalInfo
                c.SchoolSponsorshipNumber,
                c.VatNumber,
                c.RegistrationNumber,
                c.VatExemptionNumber,
                c.ChequePayableTo,
                -- OperationalRatios
                c.Guarantees,
                c.IndividualsRatio,
                c.StaffingRatio,
                c.EmptyBeds,
                c.BankBeneficiaryName,
                c.BankAccountNumber,
                c.BankName,
                c.Iban,
                c.SwiftCode,
                c.BranchCode,
                c.AbaRoutingNo,
                c.AchAba,
                c.BankStreet,
                c.BankDistrict,
                c.BankCity,
                c.BankPostalCode,
                c.BankCountryId,
                c.BeneficiaryStreet,
                c.BeneficiaryDistrict,
                c.BeneficiaryCity,
                c.BeneficiaryPostalCode,
                c.BeneficiaryCountryId,
                c.IntermediaryBankName,
                c.IntermediarySwiftCode,
                c.IntermediaryStreet,
                c.IntermediaryDistrict,
                c.IntermediaryCity,
                c.IntermediaryPostalCode,
                c.IntermediaryCountryId,
                -- Audit
                c.Version,
                c.CreatedAt,
                c.UpdatedAt,
                createdEditor.FirstName AS CreatedByFirstName,
                createdEditor.LastName  AS CreatedByLastName,
                updatedEditor.FirstName AS UpdatedByFirstName,
                updatedEditor.LastName  AS UpdatedByLastName
            FROM PricingRef.Centre c
            LEFT JOIN Edit.Editor createdEditor
                ON createdEditor.Id = c.CreatedById
            LEFT JOIN Edit.Editor updatedEditor
                ON updatedEditor.Id = c.UpdatedById
            WHERE c.Id = @Id
              AND c.IsDeleted = 0;

            SELECT
                cc.ContactType,
                cc.Name,
                cc.Email,
                cc.SignatureData,
                cc.SignatureContentType,
                cc.SignatureFileName
            FROM PricingRef.CentreContacts cc
            WHERE cc.CentreId = @Id;

            SELECT
                ctc.ContentTemplateId,
                ctc.AudienceId,
                ctc.Content,
                ctc.Format
            FROM PricingRef.CentreTextContent ctc
            WHERE ctc.CentreId = @Id
              AND ctc.IsDeleted = 0;
            """;

        await using DbConnection connection = _connectionFactory.CreateConnection();

        CommandDefinition command = new(
            commandText: sql,
            parameters: new { Id = id },
            cancellationToken: ct);

        await using SqlMapper.GridReader grid = await connection.QueryMultipleAsync(command);

        CentreDetailsRow? row = await grid.ReadSingleOrDefaultAsync<CentreDetailsRow>();

        if (row is null)
            return null;

        List<CentreContactRow> contactRows = (await grid.ReadAsync<CentreContactRow>()).AsList();
        List<TextContentDto> texts = (await grid.ReadAsync<TextContentDto>()).AsList();

        return MapDetailsRow(row, contactRows, texts);
    }

    public async Task<PagedResult<CentreListItemDto>> GetListAsync(
        string? search,
        bool? isActive,
        PagingFilter paging,
        CancellationToken ct = default)
    {
        const string sql = """
            SELECT COUNT(1)
            FROM PricingRef.Centre c
            LEFT JOIN ReferenceData.Country country
                ON country.Id = c.ContactCountryId
               AND country.IsDeleted = 0
            LEFT JOIN Edit.Editor createdEditor
                ON createdEditor.Id = c.CreatedById
            LEFT JOIN Edit.Editor updatedEditor
                ON updatedEditor.Id = c.UpdatedById
            WHERE c.IsDeleted = 0
              AND (@IsActive IS NULL OR c.IsActive = @IsActive)
              AND (
                  @Search IS NULL
                  OR c.Name  LIKE '%' + @Search + '%'
                  OR c.Code  LIKE '%' + @Search + '%'
                  OR c.ContactCity LIKE '%' + @Search + '%'
                  OR country.Name  LIKE '%' + @Search + '%'
                  OR createdEditor.FirstName LIKE '%' + @Search + '%'
                  OR createdEditor.LastName  LIKE '%' + @Search + '%'
                  OR updatedEditor.FirstName LIKE '%' + @Search + '%'
                  OR updatedEditor.LastName  LIKE '%' + @Search + '%'
              );

            SELECT
                c.Id,
                c.Name,
                c.Code,
                c.IsActive,
                c.IsPhysicalCentre,
                c.ContactCity,
                country.Name AS CountryName,
                c.CreatedAt,
                c.UpdatedAt,
                createdEditor.FirstName AS CreatedByFirstName,
                createdEditor.LastName  AS CreatedByLastName,
                updatedEditor.FirstName AS UpdatedByFirstName,
                updatedEditor.LastName  AS UpdatedByLastName
            FROM PricingRef.Centre c
            LEFT JOIN ReferenceData.Country country
                ON country.Id = c.ContactCountryId
               AND country.IsDeleted = 0
            LEFT JOIN Edit.Editor createdEditor
                ON createdEditor.Id = c.CreatedById
            LEFT JOIN Edit.Editor updatedEditor
                ON updatedEditor.Id = c.UpdatedById
            WHERE c.IsDeleted = 0
              AND (@IsActive IS NULL OR c.IsActive = @IsActive)
              AND (
                  @Search IS NULL
                  OR c.Name  LIKE '%' + @Search + '%'
                  OR c.Code  LIKE '%' + @Search + '%'
                  OR c.ContactCity LIKE '%' + @Search + '%'
                  OR country.Name  LIKE '%' + @Search + '%'
                  OR createdEditor.FirstName LIKE '%' + @Search + '%'
                  OR createdEditor.LastName  LIKE '%' + @Search + '%'
                  OR updatedEditor.FirstName LIKE '%' + @Search + '%'
                  OR updatedEditor.LastName  LIKE '%' + @Search + '%'
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
        IEnumerable<CentreListItemRow> rows = await grid.ReadAsync<CentreListItemRow>();
        List<CentreListItemDto> items = rows.Select(MapListItemRow).ToList();

        return new PagedResult<CentreListItemDto>(
            Items: items,
            TotalCount: totalCount,
            Page: page,
            PageSize: pageSize);
    }

    private static CentreDetailsDto MapDetailsRow(
        CentreDetailsRow row,
        IReadOnlyCollection<CentreContactRow> contactRows,
        IReadOnlyCollection<TextContentDto> texts)
    {
        return new CentreDetailsDto(
            row.Id,
            row.Name,
            row.Code,
            row.CurrencyId,
            row.PrintFormat,
            row.IsActive,
            row.IsPhysicalCentre,
            MapContactInfo(row),
            MapLegalInfo(row),
            MapOperationalRatios(row),
            MapBankDetails(row),
            contactRows.Select(MapContact).ToList(),
            texts,
            ToBase64Version(row.Version),
            ToDateOnly(row.CreatedAt),
            BuildEditorName(row.CreatedByFirstName, row.CreatedByLastName),
            ToDateOnly(row.UpdatedAt),
            BuildEditorName(row.UpdatedByFirstName, row.UpdatedByLastName));
    }

    private static CentreListItemDto MapListItemRow(CentreListItemRow row)
    {
        return new CentreListItemDto(
            row.Id,
            row.Name,
            row.Code,
            row.IsActive,
            row.IsPhysicalCentre,
            row.ContactCity,
            row.CountryName,
            ToDateOnly(row.CreatedAt),
            BuildEditorName(row.CreatedByFirstName, row.CreatedByLastName),
            ToDateOnly(row.UpdatedAt),
            BuildEditorName(row.UpdatedByFirstName, row.UpdatedByLastName));
    }

    private static CentreContactInfoDto MapContactInfo(CentreDetailsRow row)
    {
        return new CentreContactInfoDto(
            row.GeneralEmail,
            row.AccommodationEmail,
            row.Telephone,
            row.EmergencyTelephone,
            row.TransferEmergencyTelephone,
            row.BrandColor,
            MapAddress(row.ContactStreet, row.ContactDistrict, row.ContactCity, row.ContactPostalCode, row.ContactCountryId),
            MapImage(row.LogoData, row.LogoContentType, row.LogoFileName));
    }

    private static CentreLegalInfoDto MapLegalInfo(CentreDetailsRow row)
    {
        return new CentreLegalInfoDto(
            row.SchoolSponsorshipNumber,
            row.VatNumber,
            row.RegistrationNumber,
            row.VatExemptionNumber,
            row.ChequePayableTo);
    }

    private static CentreOperationalRatiosDto MapOperationalRatios(CentreDetailsRow row)
    {
        return new CentreOperationalRatiosDto(
            row.Guarantees,
            row.IndividualsRatio,
            row.StaffingRatio,
            row.EmptyBeds);
    }

    private static CentreBankDetailsDto MapBankDetails(CentreDetailsRow row)
    {
        return new CentreBankDetailsDto(
            row.BankBeneficiaryName,
            row.BankAccountNumber,
            row.BankName,
            row.Iban,
            row.SwiftCode,
            row.BranchCode,
            row.AbaRoutingNo,
            row.AchAba,
            MapAddress(row.BankStreet, row.BankDistrict, row.BankCity, row.BankPostalCode, row.BankCountryId)!,
            MapAddress(row.BeneficiaryStreet, row.BeneficiaryDistrict, row.BeneficiaryCity, row.BeneficiaryPostalCode, row.BeneficiaryCountryId)!,
            MapAddress(row.IntermediaryStreet, row.IntermediaryDistrict, row.IntermediaryCity, row.IntermediaryPostalCode, row.IntermediaryCountryId)!,
            row.IntermediaryBankName,
            row.IntermediarySwiftCode);
    }

    private static CentreContactDto MapContact(CentreContactRow row)
    {
        return new CentreContactDto(
            row.ContactType,
            row.Name,
            row.Email,
            MapImage(row.SignatureData, row.SignatureContentType, row.SignatureFileName)!);
    }

    private static AddressDto? MapAddress(string? street, string? district, string? city, string? postalCode, int? countryId)
    {
        bool isEmpty = street is null && district is null && city is null && postalCode is null && countryId is null;
        return isEmpty ? null : new AddressDto(street, district, city, postalCode, countryId);
    }

    private static ImageFileDto? MapImage(byte[]? data, string? contentType, string? fileName)
    {
        bool isEmpty = data is null && contentType is null && fileName is null;
        return isEmpty ? null : new ImageFileDto(data, contentType, fileName);
    }

    private static string? BuildEditorName(string? firstName, string? lastName)
    {
        string name = string.Join(" ", new[] { firstName, lastName }.Where(v => !string.IsNullOrWhiteSpace(v)));
        return string.IsNullOrWhiteSpace(name) ? null : name;
    }

    private static DateOnly ToDateOnly(DateTimeOffset value) => DateOnly.FromDateTime(value.DateTime);

    private static string ToBase64Version(byte[]? version) => Convert.ToBase64String(version ?? []);

    private sealed class CentreDetailsRow
    {
        public int Id { get; init; }
        public string Name { get; init; } = null!;
        public string Code { get; init; } = null!;
        public int CurrencyId { get; init; }
        public PrintFormat PrintFormat { get; init; }
        public bool IsActive { get; init; }
        public bool IsPhysicalCentre { get; init; }
        // ContactInfo
        public string? GeneralEmail { get; init; }
        public string? AccommodationEmail { get; init; }
        public string? Telephone { get; init; }
        public string? EmergencyTelephone { get; init; }
        public string? TransferEmergencyTelephone { get; init; }
        public string? BrandColor { get; init; }
        public string? ContactStreet { get; init; }
        public string? ContactDistrict { get; init; }
        public string? ContactCity { get; init; }
        public string? ContactPostalCode { get; init; }
        public int? ContactCountryId { get; init; }
        public byte[]? LogoData { get; init; }
        public string? LogoContentType { get; init; }
        public string? LogoFileName { get; init; }
        // LegalInfo
        public string? SchoolSponsorshipNumber { get; init; }
        public string? VatNumber { get; init; }
        public string? RegistrationNumber { get; init; }
        public string? VatExemptionNumber { get; init; }
        public string? ChequePayableTo { get; init; }
        // OperationalRatios
        public decimal? Guarantees { get; init; }
        public decimal? IndividualsRatio { get; init; }
        public decimal? StaffingRatio { get; init; }
        public decimal? EmptyBeds { get; init; }
        // BankDetails
        public string BankBeneficiaryName { get; init; } = null!;
        public string BankAccountNumber { get; init; } = null!;
        public string BankName { get; init; } = null!;
        public string? Iban { get; init; }
        public string? SwiftCode { get; init; }
        public string? BranchCode { get; init; }
        public string? AbaRoutingNo { get; init; }
        public string? AchAba { get; init; }
        public string? BankStreet { get; init; }
        public string? BankDistrict { get; init; }
        public string? BankCity { get; init; }
        public string? BankPostalCode { get; init; }
        public int? BankCountryId { get; init; }
        public string? BeneficiaryStreet { get; init; }
        public string? BeneficiaryDistrict { get; init; }
        public string? BeneficiaryCity { get; init; }
        public string? BeneficiaryPostalCode { get; init; }
        public int? BeneficiaryCountryId { get; init; }
        public string? IntermediaryBankName { get; init; }
        public string? IntermediarySwiftCode { get; init; }
        public string? IntermediaryStreet { get; init; }
        public string? IntermediaryDistrict { get; init; }
        public string? IntermediaryCity { get; init; }
        public string? IntermediaryPostalCode { get; init; }
        public int? IntermediaryCountryId { get; init; }
        // Audit
        public byte[]? Version { get; init; }
        public DateTimeOffset CreatedAt { get; init; }
        public DateTimeOffset UpdatedAt { get; init; }
        public string? CreatedByFirstName { get; init; }
        public string? CreatedByLastName { get; init; }
        public string? UpdatedByFirstName { get; init; }
        public string? UpdatedByLastName { get; init; }
    }

    private sealed class CentreContactRow
    {
        public CentreContactType ContactType { get; init; }
        public string Name { get; init; } = null!;
        public string? Email { get; init; }
        public byte[]? SignatureData { get; init; }
        public string? SignatureContentType { get; init; }
        public string? SignatureFileName { get; init; }
    }

    private sealed class CentreListItemRow
    {
        public int Id { get; init; }
        public string Name { get; init; } = null!;
        public string Code { get; init; } = null!;
        public bool IsActive { get; init; }
        public bool IsPhysicalCentre { get; init; }
        public string? ContactCity { get; init; }
        public string? CountryName { get; init; }
        public DateTimeOffset CreatedAt { get; init; }
        public DateTimeOffset UpdatedAt { get; init; }
        public string? CreatedByFirstName { get; init; }
        public string? CreatedByLastName { get; init; }
        public string? UpdatedByFirstName { get; init; }
        public string? UpdatedByLastName { get; init; }
    }
}