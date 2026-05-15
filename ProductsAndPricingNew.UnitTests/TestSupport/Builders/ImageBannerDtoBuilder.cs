using ProductsAndPricingNew.Application.Common.Models;

namespace ProductsAndPricingNew.UnitTests.TestSupport.Builders;

internal sealed class ImageBannerDtoBuilder
{
    private byte[]? _data = [1, 2, 3];
    private string? _contentType = "image/png";
    private string? _fileName = "banner.png";

    public ImageBannerDtoBuilder WithData(byte[]? data)
    {
        _data = data;
        return this;
    }

    public ImageBannerDtoBuilder WithContentType(string? contentType)
    {
        _contentType = contentType;
        return this;
    }

    public ImageBannerDtoBuilder WithFileName(string? fileName)
    {
        _fileName = fileName;
        return this;
    }

    public ImageFileDto Build()
        => new(_data, _contentType, _fileName);
}
