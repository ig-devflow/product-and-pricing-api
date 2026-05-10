using ProductsAndPricingNew.Application.Common.Models;
using ProductsAndPricingNew.Domain.ReferenceData;

namespace ProductsAndPricingNew.UnitTests.TestSupport.Builders;

internal sealed class TextContentDtoBuilder
{
    private int _contentTemplateId = 100;
    private int? _audienceId;
    private string? _content = "Text";
    private ContentFormat _format = ContentFormat.PlainText;

    public TextContentDtoBuilder WithContentTemplateId(int id)
    {
        _contentTemplateId = id;
        return this;
    }

    public TextContentDtoBuilder WithAudienceId(int? id)
    {
        _audienceId = id;
        return this;
    }

    public TextContentDtoBuilder WithContent(string? content)
    {
        _content = content;
        return this;
    }

    public TextContentDtoBuilder WithFormat(ContentFormat format)
    {
        _format = format;
        return this;
    }

    public TextContentDto Build()
        => new(_contentTemplateId, _audienceId, _content, _format);
}
