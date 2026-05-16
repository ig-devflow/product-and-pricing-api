using ProductsAndPricingNew.Domain.Common.Exceptions;
using ProductsAndPricingNew.Domain.Entities.PricingRef;
using ProductsAndPricingNew.Domain.ReferenceData;
using ProductsAndPricingNew.Domain.SharedKernel.Definitions;
using ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;

namespace ProductsAndPricingNew.UnitTests.Domain.Divisions;

public sealed class DivisionTests
{
    [Fact]
    public void Builder_WithValidData_CreatesDivision()
    {
        Division division = new Division.Builder(" Division ", " https://example.com ")
            .IsActive(true)
            .TermsAndConditions("  Terms   and conditions  ")
            .GroupsPaymentTerms("  Groups   payment terms  ")
            .HeadOfficeEmail("  office@example.com  ")
            .HeadOfficeTelephone("  +1   555   1234567  ")
            .ContactAddress(new AddressDefinition(1, " Street ", null, " City ", " 10001 "))
            .AccreditationBanner(new ImageFileDefinition([1], " IMAGE/PNG ", " banner.png "))
            .Texts([new TextContentDefinition(100, null, " Text ", ContentFormat.PlainText)])
            .Build();

        Assert.Equal("Division", division.Name);
        Assert.Equal("https://example.com", division.WebsiteUrl.ToString());
        Assert.True(division.IsActive);
        Assert.Equal("Terms and conditions", division.TermsAndConditions);
        Assert.Equal("Groups payment terms", division.GroupsPaymentTerms);
        Assert.Equal("office@example.com", division.HeadOfficeEmail.ToString());
        Assert.Equal("+1 555 1234567", division.HeadOfficeTelephoneNo.ToString());
        Assert.Equal(1, division.ContactAddress.CountryId);
        Assert.Equal("Street", division.ContactAddress.Street);
        Assert.Equal("image/png", division.AccreditationBanner.ContentType);

        DivisionTextContent text = Assert.Single(division.Texts);
        Assert.Equal(100, text.ContentTemplateId);
        Assert.Equal("Text", text.Text.Content);
    }

    [Fact]
    public void Builder_WithInvalidName_Throws()
    {
        Assert.Throws<DomainException>(() => new Division.Builder(" ", "https://example.com").Build());
    }

    [Fact]
    public void Builder_WithInvalidWebsite_Throws()
    {
        Assert.Throws<DomainException>(() => new Division.Builder("Division", "not-a-url").Build());
    }

    [Fact]
    public void Rename_WithValidName_ChangesName()
    {
        Division division = CreateDivision();

        division.Rename(" New   name ");

        Assert.Equal("New name", division.Name);
    }

    [Fact]
    public void Rename_WithEmptyName_Throws()
    {
        Division division = CreateDivision();

        Assert.Throws<DomainException>(() => division.Rename(" "));
    }

    [Fact]
    public void ChangeWebsite_WithValidWebsite_ChangesWebsite()
    {
        Division division = CreateDivision();

        division.ChangeWebsite(" https://new.example.com ");

        Assert.Equal("https://new.example.com", division.WebsiteUrl.ToString());
    }

    [Fact]
    public void ChangeWebsite_WithTooLongWebsite_Throws()
    {
        Division division = CreateDivision();
        string tooLong = new('A', WebsiteUrl.Rules.MaxLength + 1);

        Assert.Throws<DomainException>(() => division.ChangeWebsite(tooLong));
    }

    [Fact]
    public void ChangeActiveState_ChangesIsActive()
    {
        Division division = CreateDivision();

        division.ChangeActiveState(false);

        Assert.False(division.IsActive);
    }

    [Fact]
    public void ChangeContactAddress_WithAddress_ChangesAddress()
    {
        Division division = CreateDivision();

        division.ChangeContactAddress(new AddressDefinition(2, " Main   Street ", null, " Boston ", " 02108 "));
        Assert.Equal(2, division.ContactAddress.CountryId);
        Assert.Equal("Main Street", division.ContactAddress.Street);
        Assert.Equal("Boston", division.ContactAddress.City);
        Assert.Equal("02108", division.ContactAddress.PostalCode);
    }

    [Fact]
    public void ChangeContactAddress_WithNullOrEmptyAddress_ResetsToEmpty()
    {
        Division division = CreateDivision();

        division.ChangeContactAddress(new AddressDefinition(null, " ", null, null, null));

        Assert.Same(Address.Empty, division.ContactAddress);
    }

    [Fact]
    public void ChangeAccreditationBanner_WithValidImage_ChangesBanner()
    {
        Division division = CreateDivision();

        division.ChangeAccreditationBanner(new ImageFileDefinition([9], "image/webp", "banner.webp"));

        Assert.Equal(new byte[] { 9 }, division.AccreditationBanner.Data);
        Assert.Equal("image/webp", division.AccreditationBanner.ContentType);
        Assert.Equal("banner.webp", division.AccreditationBanner.FileName);
    }

    [Fact]
    public void ChangeAccreditationBanner_WithNullData_ResetsToEmpty()
    {
        Division division = CreateDivision();

        division.ChangeAccreditationBanner(new ImageFileDefinition(null, null, null));

        Assert.Same(ImageFile.Empty, division.AccreditationBanner);
    }

    [Fact]
    public void ChangeAccreditationBanner_WithTooLargeImage_Throws()
    {
        Division division = CreateDivision();
        byte[] tooLarge = new byte[Division.Rules.AccreditationBannerMaxBytes + 1];

        Assert.Throws<DomainException>(() => division.ChangeAccreditationBanner(new ImageFileDefinition(tooLarge, "image/png", "banner.png")));
    }

    [Fact]
    public void ReplaceTexts_AddsNewText()
    {
        Division division = CreateDivisionWithoutTexts();

        division.ReplaceTexts([new TextContentDefinition(100, null, "Public copy", ContentFormat.PlainText)]);

        DivisionTextContent text = Assert.Single(division.Texts);
        Assert.False(text.IsDeleted);
        Assert.Equal(100, text.ContentTemplateId);
        Assert.Equal("Public copy", text.Text.Content);
    }

    [Fact]
    public void ReplaceTexts_UpdatesExistingText()
    {
        Division division = CreateDivision(new TextContentDefinition(100, null, "Old copy", ContentFormat.PlainText));

        division.ReplaceTexts([new TextContentDefinition(100, null, "New copy", ContentFormat.PlainText)]);

        DivisionTextContent text = Assert.Single(division.Texts);
        Assert.False(text.IsDeleted);
        Assert.Equal("New copy", text.Text.Content);
    }

    [Fact]
    public void ReplaceTexts_MarksMissingExistingTextAsDeleted()
    {
        Division division = CreateDivision(
            new TextContentDefinition(100, null, "Keep me", ContentFormat.PlainText),
            new TextContentDefinition(101, null, "Delete me", ContentFormat.PlainText));

        division.ReplaceTexts([new TextContentDefinition(100, null, "Keep me", ContentFormat.PlainText)]);

        DivisionTextContent deletedText = division.Texts.Single(x => x.ContentTemplateId == 101);
        Assert.True(deletedText.IsDeleted);
    }

    [Fact]
    public void ReplaceTexts_RestoresDeletedTextWhenSameKeyAppearsAgain()
    {
        Division division = CreateDivision(new TextContentDefinition(100, null, "Original copy", ContentFormat.PlainText));

        division.ReplaceTexts([]);
        DivisionTextContent deletedText = Assert.Single(division.Texts);
        Assert.True(deletedText.IsDeleted);

        division.ReplaceTexts([new TextContentDefinition(100, null, "Restored copy", ContentFormat.PlainText)]);

        DivisionTextContent restoredText = Assert.Single(division.Texts);
        Assert.False(restoredText.IsDeleted);
        Assert.Equal("Restored copy", restoredText.Text.Content);
    }

    [Fact]
    public void ReplaceTexts_WithDuplicateKeys_Throws()
    {
        Division division = CreateDivision();
        TextContentDefinition[] texts =
        [
            new(100, null, "Public copy", ContentFormat.PlainText),
            new(100, null, "Duplicate public copy", ContentFormat.PlainText)
        ];

        Assert.Throws<DomainException>(() => division.ReplaceTexts(texts));
    }

    [Fact]
    public void ReplaceTexts_AllowsSameTemplateForDifferentAudiences()
    {
        Division division = CreateDivisionWithoutTexts();

        division.ReplaceTexts([
            new TextContentDefinition(100, null, "Public copy", ContentFormat.PlainText),
            new TextContentDefinition(100, 10, "Audience copy", ContentFormat.PlainText)
        ]);

        Assert.Equal(2, division.Texts.Count);
        Assert.Contains(division.Texts, text => text.ContentTemplateId == 100 && text.AudienceId is null);
        Assert.Contains(division.Texts, text => text.ContentTemplateId == 100 && text.AudienceId == 10);
    }

    [Fact]
    public void ReplaceTexts_TreatsAudienceIdZeroAsNull()
    {
        Division division = CreateDivisionWithoutTexts();

        division.ReplaceTexts([new TextContentDefinition(100, 0, "Public copy", ContentFormat.PlainText)]);

        DivisionTextContent text = Assert.Single(division.Texts);
        Assert.Null(text.AudienceId);
    }

    [Fact]
    public void ReplaceTexts_WithEmptyContentForExistingText_DeletesText()
    {
        Division division = CreateDivision(new TextContentDefinition(100, null, "Old copy", ContentFormat.PlainText));

        division.ReplaceTexts([new TextContentDefinition(100, null, " ", ContentFormat.None)]);

        DivisionTextContent text = Assert.Single(division.Texts);
        Assert.True(text.IsDeleted);
    }

    [Fact]
    public void ReplaceTexts_WithEmptyContentForMissingText_DoesNotCreateText()
    {
        Division division = CreateDivisionWithoutTexts();

        division.ReplaceTexts([new TextContentDefinition(100, null, " ", ContentFormat.None)]);

        Assert.Empty(division.Texts);
    }

    [Fact]
    public void ReplaceTexts_WithInvalidContentTemplateId_Throws()
    {
        Division division = CreateDivision();

        Assert.Throws<DomainException>(() => division.ReplaceTexts([
            new TextContentDefinition(0, null, "Text", ContentFormat.PlainText)
        ]));
    }

    [Fact]
    public void ReplaceTexts_WithInvalidContentFormat_Throws()
    {
        Division division = CreateDivision();

        Assert.Throws<DomainException>(() => division.ReplaceTexts([
            new TextContentDefinition(100, null, "Text", (ContentFormat)999)
        ]));
    }

    private static Division CreateDivision(params TextContentDefinition[] texts)
        => new Division.Builder("Division", "https://example.com")
            .IsActive(true)
            .ContactAddress(new AddressDefinition(1, "Street", null, "City", "10001"))
            .AccreditationBanner(new ImageFileDefinition([1], "image/png", "banner.png"))
            .Texts(texts.Length == 0
                ? [new TextContentDefinition(100, null, "Text", ContentFormat.PlainText)]
                : texts)
            .Build();

    private static Division CreateDivisionWithoutTexts()
        => new Division.Builder("Division", "https://example.com")
            .IsActive(true)
            .Build();
}
