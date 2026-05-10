using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using ProductsAndPricingNew.Domain.Entities.PricingRef;
using ProductsAndPricingNew.Domain.ReferenceData;
using ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;
using ProductsAndPricingNew.Persistence;

namespace ProductsAndPricingNew.UnitTests.Persistence.Model;

public sealed class DivisionTextContentConfigurationTests
{
    [Fact]
    public void TableNameAndSchema_AreCorrect()
    {
        using ProductsAndPricingDbContext db = CreateContext();
        IEntityType entity = GetDivisionTextContentEntity(db);

        Assert.Equal("DivisionTextContent", entity.GetTableName());
        Assert.Equal("PricingRef", entity.GetSchema());
    }

    [Fact]
    public void ContentColumn_UsesFormattedTextMaxLength()
    {
        using ProductsAndPricingDbContext db = CreateContext();
        IEntityType entity = GetDivisionTextContentEntity(db);
        IComplexProperty text = entity.FindComplexProperty(nameof(DivisionTextContent.Text))!;
        IProperty content = text.ComplexType.FindProperty(nameof(FormattedText.Content))!;

        Assert.Equal("Content", content.GetColumnName());
        Assert.Equal(FormattedText.Rules.ContentMaxLength, content.GetMaxLength());
    }

    [Fact]
    public void FormatColumn_IsConfigured()
    {
        using ProductsAndPricingDbContext db = CreateContext();
        IEntityType entity = GetDivisionTextContentEntity(db);
        IComplexProperty text = entity.FindComplexProperty(nameof(DivisionTextContent.Text))!;
        IProperty format = text.ComplexType.FindProperty(nameof(FormattedText.Format))!;

        Assert.Equal("Format", format.GetColumnName());
        Assert.Equal("smallint", format.GetColumnType());
        Assert.Equal(ContentFormat.None, format.GetDefaultValue());
        Assert.False(format.IsNullable);
    }

    [Fact]
    public void DivisionRelationship_IsConfigured()
    {
        using ProductsAndPricingDbContext db = CreateContext();
        IEntityType entity = GetDivisionTextContentEntity(db);

        IForeignKey foreignKey = entity.GetForeignKeys()
            .Single(key => key.Properties.Single().Name == nameof(DivisionTextContent.DivisionId));

        Assert.Equal(typeof(Division), foreignKey.PrincipalEntityType.ClrType);
        Assert.Equal(DeleteBehavior.Cascade, foreignKey.DeleteBehavior);
    }

    [Fact]
    public void ContentTemplateRelationship_UsesRestrictDelete()
    {
        using ProductsAndPricingDbContext db = CreateContext();
        IEntityType entity = GetDivisionTextContentEntity(db);

        IForeignKey foreignKey = entity.GetForeignKeys()
            .Single(key => key.Properties.Single().Name == nameof(DivisionTextContent.ContentTemplateId));

        Assert.Equal(typeof(ContentTemplate), foreignKey.PrincipalEntityType.ClrType);
        Assert.Equal(DeleteBehavior.Restrict, foreignKey.DeleteBehavior);
    }

    [Fact]
    public void AudienceRelationship_UsesRestrictDelete()
    {
        using ProductsAndPricingDbContext db = CreateContext();
        IEntityType entity = GetDivisionTextContentEntity(db);

        IForeignKey foreignKey = entity.GetForeignKeys()
            .Single(key => key.Properties.Single().Name == nameof(DivisionTextContent.AudienceId));

        Assert.Equal(typeof(Audience), foreignKey.PrincipalEntityType.ClrType);
        Assert.Equal(DeleteBehavior.Restrict, foreignKey.DeleteBehavior);
    }

    [Fact]
    public void UniqueFilteredIndex_ForActiveDivisionTemplateAudienceKeys_IsConfigured()
    {
        using ProductsAndPricingDbContext db = CreateContext();
        IEntityType entity = GetDivisionTextContentEntity(db);

        IIndex index = entity.GetIndexes()
            .Single(index => index.Properties.Select(property => property.Name).SequenceEqual([
                nameof(DivisionTextContent.DivisionId),
                nameof(DivisionTextContent.ContentTemplateId),
                nameof(DivisionTextContent.AudienceId)
            ]));

        Assert.True(index.IsUnique);
        Assert.Equal("[IsDeleted] = 0", index.GetFilter());
    }

    private static IEntityType GetDivisionTextContentEntity(ProductsAndPricingDbContext db)
        => db.Model.FindEntityType(typeof(DivisionTextContent))!;

    private static ProductsAndPricingDbContext CreateContext()
    {
        DbContextOptions<ProductsAndPricingDbContext> options = new DbContextOptionsBuilder<ProductsAndPricingDbContext>()
            .UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=ProductsAndPricingNewTests;Trusted_Connection=True;")
            .Options;

        return new ProductsAndPricingDbContext(options);
    }
}
