using ProductsAndPricingNew.Domain.Common.Exceptions;
using ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;

namespace ProductsAndPricingNew.UnitTests.Domain.SharedKernel;

public sealed class ImageFileTests
{
    [Fact]
    public void Create_WithNullData_ReturnsEmpty()
    {
        Assert.Same(ImageFile.Empty, ImageFile.Create(null, "image/png", "banner.png"));
        Assert.Same(ImageFile.Empty, ImageFile.Create([], "image/png", "banner.png"));
    }

    [Fact]
    public void Create_WithDataAndValidMetadata_ReturnsImageFile()
    {
        ImageFile image = ImageFile.Create([1, 2, 3], "image/png", "banner.png");

        Assert.Equal(new byte[] { 1, 2, 3 }, image.Data);
        Assert.Equal("image/png", image.ContentType);
        Assert.Equal("banner.png", image.FileName);
    }

    [Fact]
    public void Create_WithDataAndMissingContentType_Throws()
    {
        Assert.Throws<DomainException>(() => ImageFile.Create([1], " ", "banner.png"));
    }

    [Fact]
    public void Create_WithDataAndMissingFileName_Throws()
    {
        Assert.Throws<DomainException>(() => ImageFile.Create([1], "image/png", " "));
    }

    [Fact]
    public void Create_WithUnsupportedContentType_Throws()
    {
        Assert.Throws<DomainException>(() => ImageFile.Create([1], "text/plain", "banner.txt"));
    }

    [Fact]
    public void Create_WithTooLongContentType_Throws()
    {
        string tooLong = new('A', ImageFile.Rules.ContentTypeMaxLength + 1);

        Assert.Throws<DomainException>(() => ImageFile.Create([1], tooLong, "banner.png"));
    }

    [Fact]
    public void Create_WithTooLongFileName_Throws()
    {
        string tooLong = new('A', ImageFile.Rules.FileNameMaxLength + 1);

        Assert.Throws<DomainException>(() => ImageFile.Create([1], "image/png", tooLong));
    }

    [Fact]
    public void Create_WithTooLargeData_WhenMaxBytesProvided_Throws()
    {
        Assert.Throws<DomainException>(() => ImageFile.Create([1, 2], "image/png", "banner.png", maxBytes: 1));
    }

    [Theory]
    [InlineData("image/png")]
    [InlineData("image/jpeg")]
    [InlineData("image/jpg")]
    [InlineData("image/webp")]
    [InlineData("image/svg+xml")]
    public void Create_WithSupportedContentTypes_AcceptsPngJpegJpgWebpSvg(string contentType)
    {
        ImageFile image = ImageFile.Create([1], contentType, "banner.png");

        Assert.Equal(contentType, image.ContentType);
    }

    [Fact]
    public void Create_NormalizesContentTypeAndFileName()
    {
        ImageFile image = ImageFile.Create([1], " IMAGE/PNG ", "  banner   file.png  ");

        Assert.Equal("image/png", image.ContentType);
        Assert.Equal("banner file.png", image.FileName);
    }

    [Fact]
    public void Data_ReturnsCopy()
    {
        byte[] source = [1, 2, 3];
        ImageFile image = ImageFile.Create(source, "image/png", "banner.png");

        source[0] = 9;
        byte[] returned = image.Data!;
        returned[0] = 8;

        Assert.Equal(1, image.Data![0]);
    }
}
