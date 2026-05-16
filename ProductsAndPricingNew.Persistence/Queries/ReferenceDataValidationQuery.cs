using System.Data.Common;
using Dapper;
using ProductsAndPricingNew.Application.Common.Validation.Abstractions;
using ProductsAndPricingNew.Domain.ReferenceData;
using ProductsAndPricingNew.Persistence.Queries.Configuration;

namespace ProductsAndPricingNew.Persistence.Queries;

internal sealed class ReferenceDataValidationQuery : IReferenceDataValidationQuery
{
    private readonly ISqlConnectionFactory _connectionFactory;

    public ReferenceDataValidationQuery(ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlySet<int>> GetActiveCountryIdsAsync(IReadOnlyCollection<int> ids, CancellationToken ct = default)
    {
        int[] normalizedIds = GetDistinctPositiveIds(ids);

        if (normalizedIds.Length == 0)
            return new HashSet<int>();

        const string sql = """
            SELECT c.Id
            FROM ReferenceData.Country c
            WHERE c.Id IN @Ids
              AND c.IsDeleted = 0;
            """;

        await using DbConnection connection = _connectionFactory.CreateConnection();

        CommandDefinition command = new(
            commandText: sql,
            parameters: new { Ids = normalizedIds },
            cancellationToken: ct);

        IEnumerable<int> existingIds = await connection.QueryAsync<int>(command);

        return existingIds.ToHashSet();
    }

    public async Task<IReadOnlySet<int>> GetActiveAudienceIdsAsync(IReadOnlyCollection<int> ids, CancellationToken ct = default)
    {
        int[] normalizedIds = GetDistinctPositiveIds(ids);

        if (normalizedIds.Length == 0)
            return new HashSet<int>();

        const string sql = """
            SELECT a.Id
            FROM ReferenceData.Audience a
            WHERE a.Id IN @Ids
              AND a.IsDeleted = 0;
            """;

        await using DbConnection connection = _connectionFactory.CreateConnection();

        CommandDefinition command = new(
            commandText: sql,
            parameters: new { Ids = normalizedIds },
            cancellationToken: ct);

        IEnumerable<int> existingIds = await connection.QueryAsync<int>(command);

        return existingIds.ToHashSet();
    }

    public async Task<IReadOnlySet<int>> GetActiveContentTemplateIdsAsync(IReadOnlyCollection<int> ids, ContentTemplateScope scope, CancellationToken ct = default)
    {
        int[] normalizedIds = GetDistinctPositiveIds(ids);

        if (normalizedIds.Length == 0)
            return new HashSet<int>();

        const string sql = """
            SELECT ct.Id
            FROM ReferenceData.ContentTemplate ct
            WHERE ct.Id IN @Ids
              AND ct.Scope = @Scope
              AND ct.IsDeleted = 0;
            """;

        await using DbConnection connection = _connectionFactory.CreateConnection();

        CommandDefinition command = new(
            commandText: sql,
            parameters: new
            {
                Ids = normalizedIds,
                Scope = (short)scope
            },
            cancellationToken: ct);

        IEnumerable<int> existingIds = await connection.QueryAsync<int>(command);

        return existingIds.ToHashSet();
    }

    private static int[] GetDistinctPositiveIds(IReadOnlyCollection<int> ids)
    {
        return ids
            .Where(id => id > 0)
            .Distinct()
            .ToArray();
    }
}
