using ProductsAndPricingNew.Domain.Abstractions;
using ProductsAndPricingNew.Domain.Base;

namespace ProductsAndPricingNew.Domain.Entities.Products;

public sealed class TransferPortInstruction : Entity<int>, ISoftDeletable
{
    internal TransferPortInstruction(int divisionId, string instructions)
    {
        if (divisionId <= 0)
            throw new DomainException("DivisionId must be greater than zero");

        DivisionId = divisionId;
        UpdateInstructions(instructions);
        IsDeleted = false;
    }

    public int DivisionId { get; private set; }
    public string Instructions { get; private set; } = null!;
    public bool IsDeleted { get; private set; }

    internal void UpdateInstructions(string instructions)
    {
        if (string.IsNullOrWhiteSpace(instructions))
            throw new DomainException("Instructions cannot be null or empty");

        string trimmed = instructions.Trim();

        if (trimmed.Length > 4000)
            throw new DomainException("Instructions cannot exceed 4000 characters");

        Instructions = trimmed;
    }

    internal void Delete() => IsDeleted = true;
}