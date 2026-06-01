using System.Data.Common;
using Dapper;
using ProductsAndPricingNew.Application.Features.ReferenceData.Abstractions;
using ProductsAndPricingNew.Application.Features.ReferenceData.Models;
using ProductsAndPricingNew.Domain.ReferenceData;
using ProductsAndPricingNew.Persistence.Queries.Configuration;

namespace ProductsAndPricingNew.Persistence.Queries;

internal sealed class ReferenceDataQuery : IReferenceDataQuery
{
    private readonly ISqlConnectionFactory _connectionFactory;

    public ReferenceDataQuery(ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyCollection<CountryReferenceDto>> GetCountriesAsync(CancellationToken ct = default)
    {
        const string sql = """
            SELECT Id, Code, Name
            FROM ReferenceData.Country
            WHERE IsDeleted = 0
            ORDER BY Name;
            """;

        await using DbConnection connection = _connectionFactory.CreateConnection();

        CommandDefinition command = new(
            commandText: sql,
            cancellationToken: ct);

        return (await connection.QueryAsync<CountryReferenceDto>(command)).AsList();
    }

    public async Task<IReadOnlyCollection<CurrencyReferenceDto>> GetCurrenciesAsync(CancellationToken ct = default)
    {
        const string sql = """
            SELECT Id, IsoCode, Name, CAST(Symbol AS nvarchar(1)) AS Symbol
            FROM ReferenceData.Currency
            WHERE IsDeleted = 0
            ORDER BY Name;
            """;

        await using DbConnection connection = _connectionFactory.CreateConnection();

        CommandDefinition command = new(
            commandText: sql,
            cancellationToken: ct);

        return (await connection.QueryAsync<CurrencyReferenceDto>(command)).AsList();
    }

    public async Task<IReadOnlyCollection<AudienceReferenceDto>> GetAudiencesAsync(CancellationToken ct = default)
    {
        const string sql = """
            SELECT Id, Name
            FROM ReferenceData.Audience
            WHERE IsDeleted = 0
            ORDER BY Name;
            """;

        await using DbConnection connection = _connectionFactory.CreateConnection();

        CommandDefinition command = new(
            commandText: sql,
            cancellationToken: ct);

        return (await connection.QueryAsync<AudienceReferenceDto>(command)).AsList();
    }

    public async Task<IReadOnlyCollection<CentreContactTypeReferenceDto>> GetCentreContactTypesAsync(CancellationToken ct = default)
    {
        const string sql = """
           SELECT Id, Name
           FROM PricingRef.CentreContactType
           WHERE IsDeleted = 0
           ORDER BY Name;
           """;

        await using DbConnection connection = _connectionFactory.CreateConnection();

        CommandDefinition command = new(
            commandText: sql,
            cancellationToken: ct);

        return (await connection.QueryAsync<CentreContactTypeReferenceDto>(command)).AsList();
    }

    public async Task<IReadOnlyCollection<UnitTypeReferenceDto>> GetUnitTypesAsync(CancellationToken ct = default)
    {
        const string sql = """
           SELECT Id, Name, Description
           FROM ReferenceData.UnitType
           WHERE IsDeleted = 0
           ORDER BY Name;
           """;

        await using DbConnection connection = _connectionFactory.CreateConnection();

        CommandDefinition command = new(
            commandText: sql,
            cancellationToken: ct);

        return (await connection.QueryAsync<UnitTypeReferenceDto>(command)).AsList();
    }

    public async Task<IReadOnlyCollection<AccommodationTypeReferenceDto>> GetAccommodationTypesAsync(CancellationToken ct = default)
    {
        const string sql = """
           SELECT Id, Name
           FROM Product.AccommodationType
           WHERE IsDeleted = 0
           ORDER BY Name;
           """;

        await using DbConnection connection = _connectionFactory.CreateConnection();

        CommandDefinition command = new(
            commandText: sql,
            cancellationToken: ct);

        return (await connection.QueryAsync<AccommodationTypeReferenceDto>(command)).AsList();
    }

    public async Task<IReadOnlyCollection<AccommodationRoomTypeReferenceDto>> GetAccommodationRoomTypesAsync(CancellationToken ct = default)
    {
        const string sql = """
           SELECT Id, Name
           FROM Product.AccommodationRoomType
           WHERE IsDeleted = 0
           ORDER BY Name;
           """;

        await using DbConnection connection = _connectionFactory.CreateConnection();

        CommandDefinition command = new(
            commandText: sql,
            cancellationToken: ct);

        return (await connection.QueryAsync<AccommodationRoomTypeReferenceDto>(command)).AsList();
    }

    public async Task<IReadOnlyCollection<AccommodationBoardTypeReferenceDto>> GetAccommodationBoardTypesAsync(CancellationToken ct = default)
    {
        const string sql = """
           SELECT Id, Name
           FROM Product.AccommodationBoardType
           WHERE IsDeleted = 0
           ORDER BY Name;
           """;

        await using DbConnection connection = _connectionFactory.CreateConnection();

        CommandDefinition command = new(
            commandText: sql,
            cancellationToken: ct);

        return (await connection.QueryAsync<AccommodationBoardTypeReferenceDto>(command)).AsList();
    }

    public async Task<IReadOnlyCollection<AccommodationBathroomTypeReferenceDto>> GetAccommodationBathroomTypesAsync(CancellationToken ct = default)
    {
        const string sql = """
           SELECT Id, Name
           FROM Product.AccommodationBathroomType
           WHERE IsDeleted = 0
           ORDER BY Name;
           """;

        await using DbConnection connection = _connectionFactory.CreateConnection();

        CommandDefinition command = new(
            commandText: sql,
            cancellationToken: ct);

        return (await connection.QueryAsync<AccommodationBathroomTypeReferenceDto>(command)).AsList();
    }

    public async Task<IReadOnlyCollection<AccommodationRoomGradeReferenceDto>> GetAccommodationRoomGradesAsync(CancellationToken ct = default)
    {
        const string sql = """
           SELECT Id, Name
           FROM Product.AccommodationRoomGrade
           WHERE IsDeleted = 0
           ORDER BY Name;
           """;

        await using DbConnection connection = _connectionFactory.CreateConnection();

        CommandDefinition command = new(
            commandText: sql,
            cancellationToken: ct);

        return (await connection.QueryAsync<AccommodationRoomGradeReferenceDto>(command)).AsList();
    }

    public async Task<IReadOnlyCollection<ContentTemplateReferenceDto>> GetContentTemplatesAsync(ContentTemplateScope? scope, CancellationToken ct = default)
    {
        const string sql = """
            SELECT Id, Name, Description, Scope
            FROM ReferenceData.ContentTemplate
            WHERE IsDeleted = 0
              AND (@Scope IS NULL OR Scope = @Scope)
            ORDER BY Scope, Name;
            """;

        await using DbConnection connection = _connectionFactory.CreateConnection();

        CommandDefinition command = new(
            commandText: sql,
            parameters: new { Scope = scope.HasValue ? (short?)scope.Value : null },
            cancellationToken: ct);

        return (await connection.QueryAsync<ContentTemplateReferenceDto>(command)).AsList();
    }

    public async Task<IReadOnlyCollection<PrintFormatReferenceDto>> GetPrintFormatsAsync(CancellationToken ct = default)
    {
        const string sql = """
           SELECT Id, Name
           FROM PricingRef.PrintFormat
           WHERE IsDeleted = 0
           ORDER BY Name;
           """;

        await using DbConnection connection = _connectionFactory.CreateConnection();

        CommandDefinition command = new(
            commandText: sql,
            cancellationToken: ct);

        return (await connection.QueryAsync<PrintFormatReferenceDto>(command)).AsList();
    }

    public async Task<IReadOnlyCollection<TransferTypeReferenceDto>> GetTransferTypesAsync(CancellationToken ct = default)
    {
        const string sql = """
           SELECT Id, Name
           FROM Product.TransferType
           WHERE IsDeleted = 0
           ORDER BY Name;
           """;

        await using DbConnection connection = _connectionFactory.CreateConnection();

        CommandDefinition command = new(
            commandText: sql,
            cancellationToken: ct);

        return (await connection.QueryAsync<TransferTypeReferenceDto>(command)).AsList();
    }

    public async Task<IReadOnlyCollection<CourseLanguageReferenceDto>> GetCourseLanguagesAsync(CancellationToken ct = default)
    {
        const string sql = """
           SELECT Id, Name
           FROM Product.CourseLanguage
           WHERE IsDeleted = 0
           ORDER BY Name;
           """;

        await using DbConnection connection = _connectionFactory.CreateConnection();

        CommandDefinition command = new(
            commandText: sql,
            cancellationToken: ct);

        return (await connection.QueryAsync<CourseLanguageReferenceDto>(command)).AsList();
    }

    public async Task<IReadOnlyCollection<CourseIntensityReferenceDto>> GetCourseIntensitiesAsync(CancellationToken ct = default)
    {
        const string sql = """
           SELECT Id, Name
           FROM Product.CourseIntensity
           WHERE IsDeleted = 0
           ORDER BY Name;
           """;

        await using DbConnection connection = _connectionFactory.CreateConnection();

        CommandDefinition command = new(
            commandText: sql,
            cancellationToken: ct);

        return (await connection.QueryAsync<CourseIntensityReferenceDto>(command)).AsList();
    }
}