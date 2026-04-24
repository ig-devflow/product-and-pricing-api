using ProductsAndPricingNew.Domain.Base;
using ProductsAndPricingNew.Domain.Common.Errors;
using ProductsAndPricingNew.Domain.Common.Extensions;

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

    public void Rename(string name) => Name = name.AsRequiredDomainText();
    public void ChangePortType(int transferPortTypeId)
    {
        if (transferPortTypeId <= 0)
            throw new DomainException("TransferPortTypeId must be greater than zero");

        TransferPortTypeId = transferPortTypeId;
    }

    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;

    public void AddOrUpdateInstruction(int divisionId, string instructions)
    {
        if (divisionId <= 0)
            throw new DomainException("DivisionId must be greater than zero");

        TransferPortInstruction? existing = _instructions.FirstOrDefault(x => x.DivisionId == divisionId);
        if (existing is not null)
        {
            existing.UpdateInstructions(instructions);
            return;
        }

        _instructions.Add(new TransferPortInstruction(divisionId, instructions));
    }

    public void RemoveInstruction(int divisionId)
    {
        TransferPortInstruction? existing = _instructions.FirstOrDefault(x => x.DivisionId == divisionId);
        if (existing is null)
            return;

        existing.Delete();
    }

    public void AddTerminal(int number, string name, int order)
    {
        if (_terminals.Any(x => x.Number == number && !x.IsDeleted))
            throw new DomainException("Terminal with the same number already exists");

        _terminals.Add(new TransferPortTerminal(number, name, order));
    }

    public void RemoveTerminal(int number)
    {
        TransferPortTerminal? terminal = _terminals.FirstOrDefault(x => x.Number == number && !x.IsDeleted);
        if (terminal is null)
            return;

        terminal.Delete();
    }
}