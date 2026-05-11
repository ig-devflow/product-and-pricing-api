using System.Reflection;
using ProductsAndPricingNew.Domain.Common.Primitives;
using ProductsAndPricingNew.Domain.Entities.PricingRef;
using ProductsAndPricingNew.UnitTests.TestSupport.Builders;

namespace ProductsAndPricingNew.UnitTests.Domain.Common.Primitives;

public sealed class AggregateRootVersionTests
{
    [Fact]
    public void HasVersion_ReturnsTrue_WhenVersionMatches()
    {
        // Arrange
        byte[] currentVersion = [1, 2, 3, 4, 5, 6, 7, 8];
        string requestVersion = Convert.ToBase64String(currentVersion);

        Division division = new DivisionBuilder()
            .WithId(1)
            .WithTexts()
            .Build();

        SetVersion(division, currentVersion);

        // Act
        bool result = division.HasVersion(requestVersion);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void HasVersion_ReturnsFalse_WhenVersionDoesNotMatch()
    {
        // Arrange
        byte[] currentVersion = [1, 2, 3, 4, 5, 6, 7, 8];
        byte[] staleVersion = [8, 7, 6, 5, 4, 3, 2, 1];

        string requestVersion = Convert.ToBase64String(staleVersion);

        Division division = new DivisionBuilder()
            .WithId(1)
            .WithTexts()
            .Build();

        SetVersion(division, currentVersion);

        // Act
        bool result = division.HasVersion(requestVersion);

        // Assert
        Assert.False(result);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("not-base64")]
    public void HasVersion_ReturnsFalse_WhenVersionTokenIsInvalid(string? requestVersion)
    {
        // Arrange
        Division division = new DivisionBuilder()
            .WithId(1)
            .WithTexts()
            .Build();

        SetVersion(division, [1, 2, 3, 4, 5, 6, 7, 8]);

        // Act
        bool result = division.HasVersion(requestVersion!);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void HasVersion_ReturnsFalse_WhenDecodedVersionLengthIsNotEightBytes()
    {
        // Arrange
        byte[] invalidLengthVersion = [1, 2, 3];
        string requestVersion = Convert.ToBase64String(invalidLengthVersion);

        Division division = new DivisionBuilder()
            .WithId(1)
            .WithTexts()
            .Build();

        SetVersion(division, [1, 2, 3, 4, 5, 6, 7, 8]);

        // Act
        bool result = division.HasVersion(requestVersion);

        // Assert
        Assert.False(result);
    }

    private static void SetVersion<TId>(AggregateRoot<TId> aggregate, byte[] version)
    {
        PropertyInfo? property = typeof(AggregateRoot<TId>).GetProperty(nameof(AggregateRoot<TId>.Version), BindingFlags.Instance | BindingFlags.Public);

        Assert.NotNull(property);

        property.SetValue(aggregate, version);
    }
}