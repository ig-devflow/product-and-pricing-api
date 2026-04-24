using ProductsAndPricingNew.Domain.Abstractions;
using ProductsAndPricingNew.Domain.Common.Errors;
using ProductsAndPricingNew.Domain.Common.Extensions;

namespace ProductsAndPricingNew.Domain.Entities.Products;

public sealed class TransferPortTerminal : ISoftDeletable
{
    private TransferPortTerminal() { }
    public int Number { get; private set; }
    public string Name { get; private set; } = null!;
    public int Order { get; private set; }
    public bool IsDeleted { get; private set; }

    internal TransferPortTerminal(int number, string name, int order)
    {
        if (number <= 0)
            throw new DomainException("Terminal number must be greater than zero");

        Number = number;
        Rename(name);
        ChangeOrder(order);
        IsDeleted = false;
    }

    internal void Rename(string name) => Name = name.AsRequiredDomainText();
    internal void ChangeOrder(int order) => Order = order;
    internal void Delete() => IsDeleted = true;
    internal void Restore() => IsDeleted = false;
}