namespace ProductsAndPricingNew.Domain.SharedKernel.Definitions;

public sealed record ImageFileDefinition(
    byte[]? Data,
    string? ContentType,
    string? FileName)
{
    public bool IsEmpty => Data is null || Data.Length == 0;

    public static ImageFileDefinition Empty { get; } = new(null, null, null);
}