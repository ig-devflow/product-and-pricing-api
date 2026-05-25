using ProductsAndPricingNew.Domain.Common.Exceptions;
using ProductsAndPricingNew.Domain.Common.Text;
using ProductsAndPricingNew.Domain.Common.Primitives;
using ProductsAndPricingNew.Domain.Entities.Products.Definitions;

namespace ProductsAndPricingNew.Domain.Entities.Products;

public sealed class TransferPort : AggregateRoot<int>
{
    private readonly List<TransferPortTerminal> _terminals = new();
    private readonly List<TransferPortInstruction> _instructions = new();

    private TransferPort() { }

    public TransferPort(int id, string name, int transferPortTypeId)
    {
        Id = id;
        Rename(name);
        ChangePortType(transferPortTypeId);
        IsActive = true;
    }

    public string Name { get; private set; } = null!;
    public int TransferPortTypeId { get; private set; }
    public bool IsActive { get; private set; }

    public IReadOnlyCollection<TransferPortTerminal> Terminals => _terminals.AsReadOnly();
    public IReadOnlyCollection<TransferPortInstruction> Instructions => _instructions.AsReadOnly();

    public void Rename(string name) => Name = name.AsRequiredDomainText(nameof(Name), Rules.NameMaxLength);

    public void ChangePortType(int transferPortTypeId)
    {
        if (transferPortTypeId <= 0)
            throw new DomainException("TransferPortTypeId must be greater than zero");

        TransferPortTypeId = transferPortTypeId;
    }

    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;

    public void ReplaceInstructions(IEnumerable<TransferPortInstructionDefinition> instructions)
    {
        ArgumentNullException.ThrowIfNull(instructions);

        List<TransferPortInstructionDefinition> incoming = instructions.ToList();
        var incomingDivisions = new HashSet<int>();

        foreach (TransferPortInstructionDefinition def in incoming)
        {
            if (def.DivisionId <= 0)
                throw new DomainException("DivisionId must be greater than zero");

            if (!incomingDivisions.Add(def.DivisionId))
                throw new DomainException($"Duplicate instruction for division {def.DivisionId}.");
        }

        foreach (TransferPortInstruction existing in _instructions.Where(x => !x.IsDeleted && !incomingDivisions.Contains(x.DivisionId)).ToList())
            existing.Delete();

        foreach (TransferPortInstructionDefinition def in incoming)
        {
            TransferPortInstruction? existing = _instructions.FirstOrDefault(x => x.DivisionId == def.DivisionId);
            if (existing is not null)
                existing.UpdateInstructions(def.Instructions);
            else
                _instructions.Add(new TransferPortInstruction(def.DivisionId, def.Instructions));
        }
    }

    public void ReplaceTerminals(IEnumerable<TransferPortTerminalDefinition> terminals)
    {
        ArgumentNullException.ThrowIfNull(terminals);

        List<TransferPortTerminalDefinition> incoming = terminals.ToList();
        var incomingNumbers = new HashSet<int>();

        foreach (TransferPortTerminalDefinition def in incoming)
        {
            if (!incomingNumbers.Add(def.Number))
                throw new DomainException($"Duplicate terminal number {def.Number}.");
        }

        foreach (TransferPortTerminal existing in _terminals.Where(x => !x.IsDeleted && !incomingNumbers.Contains(x.Number)).ToList())
            existing.Delete();

        foreach (TransferPortTerminalDefinition def in incoming)
        {
            TransferPortTerminal? existing = _terminals.FirstOrDefault(x => x.Number == def.Number);
            if (existing is not null)
            {
                if (existing.IsDeleted)
                    existing.Restore();

                existing.Rename(def.Name);
                existing.ChangeOrder(def.Order);
            }
            else
            {
                _terminals.Add(new TransferPortTerminal(def.Number, def.Name, def.Order));
            }
        }
    }

    public static class Rules
    {
        public const int NameMaxLength = 100;
    }
}
