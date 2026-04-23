using System.Data.Common;
using Microsoft.Data.SqlClient;

namespace ProductsAndPricingNew.Persistence.Queries.Configuration;

internal sealed class SqlConnectionFactory : ISqlConnectionFactory
{
    private readonly string _connectionString;

    public SqlConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public DbConnection CreateConnection()
        => new SqlConnection(_connectionString);
}