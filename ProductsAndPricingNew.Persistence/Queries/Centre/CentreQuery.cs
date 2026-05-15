using System.Data.Common;
using Dapper;
using ProductsAndPricingNew.Application.Features.Centre.Abstractions;
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
}