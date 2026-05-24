using System.Data.Common;
using Dapper;
using ProductsAndPricingNew.Domain.Common.Exceptions;
using ProductsAndPricingNew.Domain.UnitOfMeasure;
using ProductsAndPricingNew.Persistence.Queries.Configuration;

namespace ProductsAndPricingNew.Persistence.UnitOfMeasure;

internal sealed class UnitTypeProvider : IUnitTypeProvider
{
    private readonly ISqlConnectionFactory _connectionFactory;
    private readonly Lazy<IReadOnlyDictionary<int, UnitType>> _cache;

    public UnitTypeProvider(ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
        _cache = new Lazy<IReadOnlyDictionary<int, UnitType>>(Load, LazyThreadSafetyMode.ExecutionAndPublication);
    }

    public bool Exists(int unitTypeId) =>
        _cache.Value.ContainsKey(unitTypeId);

    public UnitType Get(int unitTypeId) =>
        _cache.Value.TryGetValue(unitTypeId, out UnitType? unitType)
            ? unitType
            : throw new DomainException($"UnitType '{unitTypeId}' is unknown.");

    public IReadOnlyCollection<UnitType> GetAll() => (IReadOnlyCollection<UnitType>)_cache.Value.Values;

    private IReadOnlyDictionary<int, UnitType> Load()
    {
        const string sql = """
            SELECT Id,
                   Name,
                   IsDateBased,
                   CalculationKind,
                   MinMajorUnits,
                   MinMinorUnits,
                   IsDeleted
            FROM ReferenceData.UnitType;
            """;

        using DbConnection connection = _connectionFactory.CreateConnection();
        IEnumerable<UnitType> rows = connection.Query<UnitType>(sql);
        return rows.ToDictionary(unitType => unitType.Id);
    }
}