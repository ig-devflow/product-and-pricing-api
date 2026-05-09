using ProductsAndPricingNew.Domain.Common.Exceptions;
using ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;

namespace ProductsAndPricingNew.UnitTests.Domain;

public sealed class ImageFileTests
{
    [Fact]
    public void Create_WithNullOrEmptyData_ReturnsEmpty()
    {
        Assert.Same(ImageFile.Empty, ImageFile.Create(null, "image/png", "banner.png"));
        Assert.Same(ImageFile.Empty, ImageFile.Create(Array.Empty<byte>(), "image/png", "banner.png"));
    }

    [Fact]
    public void Create_WithUnsupportedContentType_Throws()
    {
        Assert.Throws<DomainException>(() => ImageFile.Create([1], "text/plain", "banner.txt"));
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
