using ProductsAndPricingNew.Domain.Abstractions;
using ProductsAndPricingNew.Domain.Common.Exceptions;
using ProductsAndPricingNew.Domain.Common.Text;
using ProductsAndPricingNew.Domain.Common.Primitives;

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
        string normalizedInstructions = instructions.AsRequiredDomainText(nameof(Instructions), Rules.InstructionsMaxLength);
        Instructions = normalizedInstructions;
    }

    internal void Delete() => IsDeleted = true;

    public static class Rules
    {
        public const int NameMaxLength = 100;
        public const int InstructionsMaxLength = 4000;
    }
}