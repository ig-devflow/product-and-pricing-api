using System.Data.Common;
using Dapper;
using ProductsAndPricingNew.Application.Features.School.Abstractions;
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
}