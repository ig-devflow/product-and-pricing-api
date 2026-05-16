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

    public async Task<IReadOnlyCollection<ContentTemplateReferenceDto>> GetContentTemplatesAsync(
        ContentTemplateScope? scope,
        CancellationToken ct = default)
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
}
