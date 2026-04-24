using ProductsAndPricingNew.Domain.Common.Errors;
using ProductsAndPricingNew.Domain.Common.Extensions;

namespace ProductsAndPricingNew.Domain.Entities.Common;

public sealed class ImageFile : IEquatable<ImageFile>
{
    public byte[]? Data { get; }
    public string? ContentType { get; }
    public string? FileName { get; }

    private ImageFile(byte[]? data, string? contentType, string? fileName)
    {
        Data = data;
        ContentType = contentType;
        FileName = fileName;
    }

    public static ImageFile Create(byte[] data, string contentType, string fileName, int? maxBytes = null)
    {
        if (data is null || data.Length == 0)
            throw new DomainException("Image data is required.");

        if (maxBytes.HasValue && data.Length > maxBytes)
            throw new DomainException("Image is too large.");

        string? normalizedContentType = contentType.AsOptionalDomainText();
        string? normalizedFileName = fileName.AsOptionalDomainText();

        if (string.IsNullOrWhiteSpace(normalizedContentType))
            throw new DomainException("Image content type is required.");

        if (normalizedContentType is not "image/png" and not "image/jpeg" and not "image/jpg" and not "image/webp" and not "image/svg+xml")
            throw new DomainException("Unsupported image content type.");

        if (string.IsNullOrWhiteSpace(normalizedFileName))
            throw new DomainException("Image file name is required.");

        return new ImageFile(data, normalizedContentType, normalizedFileName);
    }

    public static ImageFile Empty { get; } = new(null, null, null);

    public bool Equals(ImageFile? other)
    {
        if (ReferenceEquals(this, other))
            return true;

        if (other is null)
            return false;

        return string.Equals(ContentType, other.ContentType, StringComparison.Ordinal) &&
               string.Equals(FileName, other.FileName, StringComparison.Ordinal) &&
               BytesEqual(Data, other.Data);
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

        if (Data is not null)
        {
            foreach (byte value in Data)
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
}