using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using ProductsAndPricingNew.Domain.Edit;
using ProductsAndPricingNew.Persistence;

namespace ProductsAndPricingNew.UnitTests.Persistence.Model;

public sealed class EditorConfigurationTests
{
    [Fact]
    public void TableNameAndSchema_AreCorrect()
    {
        using ProductsAndPricingDbContext db = CreateContext();
        IEntityType editor = GetEditorEntity(db);

        Assert.Equal("Editor", editor.GetTableName());
        Assert.Equal("Edit", editor.GetSchema());
    }

    [Fact]
    public void ScalarProperties_UseDomainMaxLengths()
    {
        using ProductsAndPricingDbContext db = CreateContext();
        IEntityType editor = GetEditorEntity(db);

        Assert.Equal(Editor.Rules.UserNameMaxLength, editor.FindProperty(nameof(Editor.UserName))!.GetMaxLength());
        Assert.Equal(Editor.Rules.FirstNameMaxLength, editor.FindProperty(nameof(Editor.FirstName))!.GetMaxLength());
        Assert.Equal(Editor.Rules.LastNameMaxLength, editor.FindProperty(nameof(Editor.LastName))!.GetMaxLength());
        Assert.Equal(Editor.Rules.EmailMaxLength, editor.FindProperty(nameof(Editor.Email))!.GetMaxLength());
    }

    [Fact]
    public void Id_IsNotDatabaseGenerated()
    {
        using ProductsAndPricingDbContext db = CreateContext();
        IEntityType editor = GetEditorEntity(db);

        Assert.Equal(ValueGenerated.Never, editor.FindProperty(nameof(Editor.Id))!.ValueGenerated);
    }

    private static IEntityType GetEditorEntity(ProductsAndPricingDbContext db)
        => db.Model.FindEntityType(typeof(Editor))!;

    private static ProductsAndPricingDbContext CreateContext()
    {
        DbContextOptions<ProductsAndPricingDbContext> options = new DbContextOptionsBuilder<ProductsAndPricingDbContext>()
            .UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=ProductsAndPricingNewTests;Trusted_Connection=True;")
            .Options;

        return new ProductsAndPricingDbContext(options);
    }
}
