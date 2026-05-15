using ProductsAndPricingNew.Domain.Common.Exceptions;
using ProductsAndPricingNew.Domain.Common.Text;
using ProductsAndPricingNew.Domain.SharedKernel.Definitions;

namespace ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;

public sealed class ImageFile : IEquatable<ImageFile>, IEmptyValueObject
{
    private byte[]? _data;

    public byte[]? Data => _data?.ToArray();
    public string? ContentType { get; }
    public string? FileName { get; }

    public bool IsEmpty => _data is null || _data.Length == 0;
    public static ImageFile Empty { get; } = new(null, null, null);

    private ImageFile() { }

    private ImageFile(byte[]? data, string? contentType, string? fileName)
    {
        _data = data?.ToArray();
        ContentType = contentType;
        FileName = fileName;
    }

    public static ImageFile Create(ImageFileDefinition? definition, int? maxBytes = null)
    {
        if (definition is null || definition.IsEmpty)
            return Empty;

        if (maxBytes.HasValue && definition.Data!.Length > maxBytes)
            throw new DomainException("Image is too large.");

        string normalizedContentType = definition.ContentType.AsRequiredDomainText(nameof(ContentType), Rules.ContentTypeMaxLength).ToLowerInvariant();
        string normalizedFileName = definition.FileName.AsRequiredDomainText(nameof(FileName), Rules.FileNameMaxLength);

        if (!IsSupportedContentType(normalizedContentType))
            throw new DomainException("Unsupported image content type.");

        return new ImageFile(definition.Data, normalizedContentType, normalizedFileName);
    }

    public static bool IsSupportedContentType(string? contentType)
        => !string.IsNullOrWhiteSpace(contentType) && Rules.AllowedImageContentTypes.Contains(contentType.Trim());

    public bool Equals(ImageFile? other)
    {
        if (ReferenceEquals(this, other))
            return true;

        if (other is null)
            return false;

        return string.Equals(ContentType, other.ContentType, StringComparison.Ordinal) &&
               string.Equals(FileName, other.FileName, StringComparison.Ordinal) &&
               BytesEqual(_data, other._data);
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as ImageFile);
    }

    public override int GetHashCode()
    {
        HashCode hash = new();

        hash.Add(ContentType, StringComparer.Ordinal);
        hash.Add(FileName, StringComparer.Ordinal);

        if (_data is not null)
        {
            foreach (byte value in _data)
                hash.Add(value);
        }

        return hash.ToHashCode();
    }

    private static bool BytesEqual(byte[]? left, byte[]? right)
    {
        if (ReferenceEquals(left, right))
            return true;

        if (left is null || right is null)
            return false;

        if (left.Length != right.Length)
            return false;

        for (int i = 0; i < left.Length; i++)
        {
            if (left[i] != right[i])
                return false;
        }

        return true;
    }

    public static class Rules
    {
        public const int ContentTypeMaxLength = 100;
        public const int FileNameMaxLength = 255;

        internal static readonly HashSet<string> AllowedImageContentTypes = new(StringComparer.OrdinalIgnoreCase)
        {
            "image/png",
            "image/jpeg",
            "image/jpg",
            "image/webp",
            "image/svg+xml"
        };
    }
}
