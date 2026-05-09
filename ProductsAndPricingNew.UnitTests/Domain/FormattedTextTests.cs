using ProductsAndPricingNew.Domain.Common.Exceptions;
using ProductsAndPricingNew.Domain.ReferenceData;
using ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;

namespace ProductsAndPricingNew.UnitTests.Domain;

public sealed class FormattedTextTests
{
    [Fact]
    public void Create_WithEmptyContentAndNoneFormat_ReturnsEmpty()
    {
        FormattedText text = FormattedText.Create(" ", ContentFormat.None);

        Assert.Same(FormattedText.Empty, text);
    }

    [Fact]
    public void Create_WithEmptyContentAndNonNoneFormat_Throws()
    {
        Assert.Throws<DomainException>(() => FormattedText.Create(" ", ContentFormat.PlainText));
    }

    [Fact]
    public void Create_WithContentAndNoneFormat_Throws()
    {
        Assert.Throws<DomainException>(() => FormattedText.Create("Content", ContentFormat.None));
    }

    [Fact]
    public void Create_WithContentAndFormat_NormalizesContent()
    {
        FormattedText text = FormattedText.Create("  Hello   world  ", ContentFormat.PlainText);

        Assert.Equal("Hello world", text.Content);
        Assert.Equal(ContentFormat.PlainText, text.Format);
    }
}
