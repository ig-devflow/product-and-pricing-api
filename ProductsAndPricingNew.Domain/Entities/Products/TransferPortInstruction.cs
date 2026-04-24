using ProductsAndPricingNew.Domain.Abstractions;
using ProductsAndPricingNew.Domain.Base;
using ProductsAndPricingNew.Domain.Common.Errors;
using ProductsAndPricingNew.Domain.Common.Extensions;

namespace ProductsAndPricingNew.Domain.Entities.Products;

public sealed class TransferPortInstruction : Entity<int>, ISoftDeletable
{
    private TransferPortInstruction() { }
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
        string normalizedInstructions = instructions.AsRequiredDomainText();

        if (normalizedInstructions.Length > 4000)
            throw new DomainException("Instructions cannot exceed 4000 characters");

        Instructions = normalizedInstructions;
    }

    internal void Delete() => IsDeleted = true;
}