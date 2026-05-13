using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using ProductsAndPricingNew.Domain.Entities.PricingRef;
using ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;
using ProductsAndPricingNew.Persistence;

namespace ProductsAndPricingNew.UnitTests.Persistence.Model;

public sealed class DivisionConfigurationTests
{
    private static readonly StoreObjectIdentifier DivisionTable = StoreObjectIdentifier.Table("Division", "PricingRef");

    [Fact]
    public void TableNameAndSchema_AreCorrect()
    {
        using ProductsAndPricingDbContext db = CreateContext();
        IEntityType division = GetDivisionEntity(db);

        Assert.Equal("Division", division.GetTableName());
        Assert.Equal("PricingRef", division.GetSchema());
    }

    // [Fact]
    // public void ScalarProperties_UseDomainMaxLengths()
    // {
    //     using ProductsAndPricingDbContext db = CreateContext();
    //     IEntityType division = GetDivisionEntity(db);
    //
    //     Assert.Equal(Division.Rules.NameMaxLength, division.FindProperty(nameof(Division.Name))!.GetMaxLength());
    //     Assert.Equal(WebsiteUrl.Rules.MaxLength, division.FindProperty(nameof(Division.WebsiteUrl))!.GetMaxLength());
    //     Assert.Equal(Division.Rules.TermsAndConditionsMaxLength, division.FindProperty(nameof(Division.TermsAndConditions))!.GetMaxLength());
    //     Assert.Equal(Division.Rules.GroupsPaymentTermsMaxLength, division.FindProperty(nameof(Division.GroupsPaymentTerms))!.GetMaxLength());
    //     Assert.Equal(EmailAddress.Rules.MaxLength, division.FindProperty(nameof(Division.HeadOfficeEmail))!.GetMaxLength());
    //     Assert.Equal(TelephoneNumber.Rules.MaxLength, division.FindProperty(nameof(Division.HeadOfficeTelephoneNo))!.GetMaxLength());
    // }

    [Fact]
    public void AddressColumns_UseExpectedMaxLengths()
    {
        using ProductsAndPricingDbContext db = CreateContext();
        IEntityType division = GetDivisionEntity(db);
        IComplexProperty address = division.FindComplexProperty(nameof(Division.ContactAddress))!;

        Assert.Equal(Address.Rules.AddressFieldMaxLength, address.ComplexType.FindProperty(nameof(Address.Street))!.GetMaxLength());
        Assert.Equal(Address.Rules.AddressFieldMaxLength, address.ComplexType.FindProperty(nameof(Address.District))!.GetMaxLength());
        Assert.Equal(Address.Rules.AddressFieldMaxLength, address.ComplexType.FindProperty(nameof(Address.City))!.GetMaxLength());
        Assert.Equal(Address.Rules.AddressFieldMaxLength, address.ComplexType.FindProperty(nameof(Address.PostalCode))!.GetMaxLength());
    }

    [Fact]
    public void BannerColumns_AreConfigured()
    {
        using ProductsAndPricingDbContext db = CreateContext();
        IEntityType division = GetDivisionEntity(db);
        IComplexProperty banner = division.FindComplexProperty(nameof(Division.AccreditationBanner))!;

        IProperty data = banner.ComplexType.FindProperty(nameof(ImageFile.Data))!;
        IProperty contentType = banner.ComplexType.FindProperty(nameof(ImageFile.ContentType))!;
        IProperty fileName = banner.ComplexType.FindProperty(nameof(ImageFile.FileName))!;

        Assert.Equal("BannerData", data.GetColumnName(DivisionTable));
        Assert.Equal("varbinary(max)", data.GetColumnType());
        Assert.Equal("BannerContentType", contentType.GetColumnName(DivisionTable));
        Assert.Equal(ImageFile.Rules.ContentTypeMaxLength, contentType.GetMaxLength());
        Assert.Equal("BannerFileName", fileName.GetColumnName(DivisionTable));
        Assert.Equal(ImageFile.Rules.FileNameMaxLength, fileName.GetMaxLength());
    }

    [Fact]
    public void RowVersion_IsConfiguredAsConcurrencyToken()
    {
        using ProductsAndPricingDbContext db = CreateContext();
        IEntityType division = GetDivisionEntity(db);
        IProperty rowVersion = division.FindProperty(nameof(Division.Version))!;

        Assert.True(rowVersion.IsConcurrencyToken);
        Assert.Equal(ValueGenerated.OnAddOrUpdate, rowVersion.ValueGenerated);
    }

    [Fact]
    public void IsDeleted_DefaultValueIsFalse()
    {
        using ProductsAndPricingDbContext db = CreateContext();
        IEntityType division = GetDivisionEntity(db);

        Assert.Equal(false, division.FindProperty(nameof(Division.IsDeleted))!.GetDefaultValue());
    }

    private static IEntityType GetDivisionEntity(ProductsAndPricingDbContext db)
        => db.Model.FindEntityType(typeof(Division))!;

    private static ProductsAndPricingDbContext CreateContext()
    {
        DbContextOptions<ProductsAndPricingDbContext> options = new DbContextOptionsBuilder<ProductsAndPricingDbContext>()
            .UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=ProductsAndPricingNewTests;Trusted_Connection=True;")
            .Options;

        return new ProductsAndPricingDbContext(options);
    }
}
