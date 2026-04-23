using System.Data.Common;

namespace ProductsAndPricingNew.Persistence.Queries.Configuration;

public interface ISqlConnectionFactory
{
    DbConnection CreateConnection();
}