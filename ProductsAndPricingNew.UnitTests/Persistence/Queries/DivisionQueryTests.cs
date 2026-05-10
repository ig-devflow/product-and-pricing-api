using ProductsAndPricingNew.Persistence.Queries.Division;

namespace ProductsAndPricingNew.UnitTests.Persistence.Queries;

public sealed class DivisionQueryTests
{
    [Fact]
    public void ToBase64Version_ReturnsBase64RowVersion()
    {
        byte[] rowVersion = [1, 2, 3, 4, 5, 6, 7, 8];

        string version = DivisionQuery.ToBase64Version(rowVersion);

        Assert.False(string.IsNullOrWhiteSpace(version));
        Assert.Equal(rowVersion, Convert.FromBase64String(version));
        Assert.Equal(8, Convert.FromBase64String(version).Length);
    }
}