using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using ProductsAndPricingNew.Domain.Entities.PricingRef;
using ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;
using ProductsAndPricingNew.Persistence;

namespace ProductsAndPricingNew.UnitTests.Persistence;

public sealed class DivisionConfigurationTests
{
    [Fact]
    public void DivisionModel_UsesDomainMaxLengths()
    {
        using ProductsAndPricingDbContext db = CreateContext();
        IEntityType division = db.Model.FindEntityType(typeof(Division))!;

        Assert.Equal(Division.Rules.NameMaxLength, division.FindProperty(nameof(Division.Name))!.GetMaxLength());
        Assert.Equal(Division.Rules.WebsiteUrlMaxLength, division.FindProperty(nameof(Division.WebsiteUrl))!.GetMaxLength());
        Assert.Equal(Division.Rules.HeadOfficeEmailMaxLength, division.FindProperty(nameof(Division.HeadOfficeEmail))!.GetMaxLength());
        Assert.Equal(Division.Rules.HeadOfficeTelephoneMaxLength, division.FindProperty(nameof(Division.HeadOfficeTelephoneNo))!.GetMaxLength());
        Assert.Equal(Division.Rules.TermsAndConditionsMaxLength, division.FindProperty(nameof(Division.TermsAndConditions))!.GetMaxLength());
        Assert.Equal(Division.Rules.GroupsPaymentTermsMaxLength, division.FindProperty(nameof(Division.GroupsPaymentTerms))!.GetMaxLength());

        IComplexProperty address = division.FindComplexProperty(nameof(Division.ContactAddress))!;
        Assert.Equal(Address.Rules.AddressFieldMaxLength, address.ComplexType.FindProperty(nameof(Address.Street))!.GetMaxLength());
        Assert.Equal(Address.Rules.AddressFieldMaxLength, address.ComplexType.FindProperty(nameof(Address.District))!.GetMaxLength());
        Assert.Equal(Address.Rules.AddressFieldMaxLength, address.ComplexType.FindProperty(nameof(Address.City))!.GetMaxLength());
        Assert.Equal(Address.Rules.AddressFieldMaxLength, address.ComplexType.FindProperty(nameof(Address.PostalCode))!.GetMaxLength());

        IComplexProperty banner = division.FindComplexProperty(nameof(Division.AccreditationBanner))!;
        Assert.Equal(ImageFile.Rules.ContentTypeMaxLength, banner.ComplexType.FindProperty(nameof(ImageFile.ContentType))!.GetMaxLength());
        Assert.Equal(ImageFile.Rules.FileNameMaxLength, banner.ComplexType.FindProperty(nameof(ImageFile.FileName))!.GetMaxLength());
    }

    [Fact]
    public void DivisionModel_ConfiguresBannerColumnsOnDivisionTable()
    {
        using ProductsAndPricingDbContext db = CreateContext();
        IEntityType division = db.Model.FindEntityType(typeof(Division))!;
        StoreObjectIdentifier table = StoreObjectIdentifier.Table("Division", "PricingRef");
        IComplexProperty banner = division.FindComplexProperty(nameof(Division.AccreditationBanner))!;

        Assert.Equal("BannerData", banner.ComplexType.FindProperty(nameof(ImageFile.Data))!.GetColumnName(table));
        Assert.Equal("BannerContentType", banner.ComplexType.FindProperty(nameof(ImageFile.ContentType))!.GetColumnName(table));
        Assert.Equal("BannerFileName", banner.ComplexType.FindProperty(nameof(ImageFile.FileName))!.GetColumnName(table));
    }

    private static ProductsAndPricingDbContext CreateContext()
    {
        DbContextOptions<ProductsAndPricingDbContext> options = new DbContextOptionsBuilder<ProductsAndPricingDbContext>()
            .UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=ProductsAndPricingNewTests;Trusted_Connection=True;")
            .Options;

        return new ProductsAndPricingDbContext(options);
    }
}
