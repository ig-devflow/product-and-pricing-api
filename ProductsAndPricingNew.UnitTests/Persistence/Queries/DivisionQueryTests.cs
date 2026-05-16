using ProductsAndPricingNew.Application.Features.Division.Models;
using ProductsAndPricingNew.Persistence.Queries;

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

    [Fact]
    public void BuildEditorName_ReturnsTrimmedFullName()
    {
        string? name = DivisionQuery.BuildEditorName(" System ", " User ");

        Assert.Equal("System User", name);
    }

    [Theory]
    [InlineData("", " ")]
    [InlineData("System", "User")]
    public void BuildEditorName_ReturnsExpectedName(string? firstName, string? lastName)
    {
        string? name = DivisionQuery.BuildEditorName(firstName, lastName);
        string? expected = string.Join(" ", new[] { firstName, lastName }.Select(value => value!.Trim()));

        expected = string.IsNullOrWhiteSpace(expected) ? null : expected;

        Assert.Equal(expected, name);
    }

    [Fact]
    public void MapListItemRow_MapsAdminListMetadata()
    {
        DateTimeOffset createdAt = new(2026, 5, 10, 14, 8, 0, TimeSpan.Zero);
        DateTimeOffset updatedAt = new(2026, 5, 10, 15, 30, 0, TimeSpan.Zero);

        DivisionQuery.DivisionListItemRow row = new()
        {
            Id = 12,
            Name = "Test Division",
            IsActive = true,
            WebsiteUrl = "https://test.example.com",
            HeadOfficeEmail = "office@example.com",
            City = "London",
            CountryName = "United Kingdom",
            CreatedAt = createdAt,
            UpdatedAt = updatedAt,
            CreatedByFirstName = "System",
            CreatedByLastName = "User",
            UpdatedByFirstName = "Tomas",
            UpdatedByLastName = "Bong"
        };

        DivisionListItemDto dto = DivisionQuery.MapListItemRow(row);

        Assert.Equal(12, dto.Id);
        Assert.Equal("Test Division", dto.Name);
        Assert.True(dto.IsActive);
        Assert.Equal("https://test.example.com", dto.WebsiteUrl);
        Assert.Equal("office@example.com", dto.HeadOfficeEmail);
        Assert.Equal("London", dto.City);
        Assert.Equal("United Kingdom", dto.CountryName);
        Assert.Equal(DateOnly.FromDateTime(createdAt.DateTime), dto.CreatedAt);
        Assert.Equal("System User", dto.CreatedByName);
        Assert.Equal(DateOnly.FromDateTime(updatedAt.DateTime), dto.UpdatedAt);
        Assert.Equal("Tomas Bong", dto.UpdatedByName);
    }

    [Fact]
    public void MapDetailsRow_MapsAuditMetadata()
    {
        DateTimeOffset createdAt = new(2026, 5, 10, 14, 8, 0, TimeSpan.Zero);
        DateTimeOffset updatedAt = new(2026, 5, 10, 15, 30, 0, TimeSpan.Zero);

        DivisionQuery.DivisionDetailsRow row = new()
        {
            Id = 12,
            Name = "Test Division",
            IsActive = true,
            CreatedAt = createdAt,
            UpdatedAt = updatedAt,
            CreatedByFirstName = "System",
            CreatedByLastName = "User",
            UpdatedByFirstName = "Tomas",
            UpdatedByLastName = "Bong",
            Version = [1, 2, 3, 4, 5, 6, 7, 8]
        };

        DivisionDetailsDto dto = DivisionQuery.MapDetailsRow(row, []);

        Assert.Equal(DateOnly.FromDateTime(createdAt.DateTime), dto.CreatedAt);
        Assert.Equal("System User", dto.CreatedByName);
        Assert.Equal(DateOnly.FromDateTime(updatedAt.DateTime), dto.UpdatedAt);
        Assert.Equal("Tomas Bong", dto.UpdatedByName);
    }
}