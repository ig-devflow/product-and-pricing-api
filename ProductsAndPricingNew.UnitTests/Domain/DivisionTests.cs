using ProductsAndPricingNew.Domain.Common.Exceptions;
using ProductsAndPricingNew.Domain.Entities.PricingRef;
using ProductsAndPricingNew.Domain.ReferenceData;
using ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;

namespace ProductsAndPricingNew.UnitTests.Domain;

public sealed class DivisionTests
{
    [Fact]
    public void Build_WithEmptyName_Throws()
    {
        Assert.Throws<DomainException>(() => new Division.Builder(" ", "https://example.com").Build());
    }

    [Fact]
    public void Build_WithEmptyWebsite_Throws()
    {
        Assert.Throws<DomainException>(() => new Division.Builder("Division", " ").Build());
    }

    [Fact]
    public void Build_NormalizesOptionalStrings()
    {
        Division division = new Division.Builder(" Division ", " https://example.com ")
            .TermsAndConditions("  Terms   and conditions  ")
            .GroupsPaymentTerms("  Groups   payment terms  ")
            .HeadOfficeEmail("  office@example.com  ")
            .HeadOfficeTelephone("  +1   555   1234567  ")
            .Build();

        Assert.Equal("Division", division.Name);
        Assert.Equal("https://example.com", division.WebsiteUrl);
        Assert.Equal("Terms and conditions", division.TermsAndConditions);
        Assert.Equal("Groups payment terms", division.GroupsPaymentTerms);
        Assert.Equal("office@example.com", division.HeadOfficeEmail);
        Assert.Equal("+1 555 1234567", division.HeadOfficeTelephoneNo);
    }

    [Fact]
    public void Build_WithTooLongDomainOwnedField_Throws()
    {
        string tooLongTerms = new('A', Division.Rules.TermsAndConditionsMaxLength + 1);

        Assert.Throws<DomainException>(() => new Division.Builder("Division", "https://example.com")
            .TermsAndConditions(tooLongTerms)
            .Build());
    }

    [Fact]
    public void ChangeTexts_WithDuplicateKeys_Throws()
    {
        Division division = CreateDivision();
        TextContentDefinition[] texts =
        [
            new(1, null, "Public copy", ContentFormat.PlainText),
            new(1, 0, "Also public copy", ContentFormat.PlainText)
        ];

        Assert.Throws<DomainException>(() => division.ReplaceTexts(texts));
    }

    [Fact]
    public void ChangeTexts_UpdatesExistingText()
    {
        Division division = CreateDivision(new TextContentDefinition(1, null, "Old copy", ContentFormat.PlainText));

        division.ReplaceTexts([new TextContentDefinition(1, null, "New copy", ContentFormat.PlainText)]);

        DivisionTextContent text = Assert.Single(division.Texts);
        Assert.False(text.IsDeleted);
        Assert.Equal("New copy", text.Text.Content);
    }

    [Fact]
    public void ChangeTexts_MarksMissingExistingTextAsDeleted()
    {
        Division division = CreateDivision(
            new TextContentDefinition(1, null, "Keep me", ContentFormat.PlainText),
            new TextContentDefinition(2, null, "Delete me", ContentFormat.PlainText));

        division.ReplaceTexts([new TextContentDefinition(1, null, "Keep me", ContentFormat.PlainText)]);

        DivisionTextContent deletedText = division.Texts.Single(x => x.ContentTemplateId == 2);
        Assert.True(deletedText.IsDeleted);
    }

    [Fact]
    public void ChangeTexts_RestoresDeletedTextWhenSameKeyAppearsAgain()
    {
        Division division = CreateDivision(new TextContentDefinition(1, null, "Original copy", ContentFormat.PlainText));

        division.ReplaceTexts(Array.Empty<TextContentDefinition>());
        DivisionTextContent deletedText = Assert.Single(division.Texts);
        Assert.True(deletedText.IsDeleted);

        division.ReplaceTexts([new TextContentDefinition(1, null, "Restored copy", ContentFormat.PlainText)]);

        DivisionTextContent restoredText = Assert.Single(division.Texts);
        Assert.False(restoredText.IsDeleted);
        Assert.Equal("Restored copy", restoredText.Text.Content);
    }

    private static Division CreateDivision(params TextContentDefinition[] texts)
        => new Division.Builder("Division", "https://example.com")
            .Texts(texts)
            .Build();
}
