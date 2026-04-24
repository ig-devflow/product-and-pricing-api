using ProductsAndPricingNew.Domain.Base;
using ProductsAndPricingNew.Domain.Common.Errors;
using ProductsAndPricingNew.Domain.Common.Extensions;

namespace ProductsAndPricingNew.Domain.Entities.Products;

public sealed class Transfer : AggregateRoot<int>, IProductDefinition
{
    private Transfer() { }

    public Transfer(int id, int divisionId, string name, int transferTypeId, int transferPortId, int unitTypeId)
    {
        Id = id;
        DivisionId = divisionId;
        TransferTypeId = transferTypeId;
        TransferPortId = transferPortId;
        UnitTypeId = unitTypeId;
        IsActive = true;
        FinanceCodes = new FinanceCodes(null, null);

        Rename(name);
    }

    public int DivisionId { get; private set; }
    public int UnitTypeId { get; private set; }
    public string Name { get; private set; } = null!;
    public bool IsActive { get; private set; }
    public int TransferTypeId { get; private set; }
    public int TransferPortId { get; private set; }
    public TimeOnly? TimeFrom { get; private set; }
    public TimeOnly? TimeTo { get; private set; }
    public int? AccountCategoryId { get; private set; }
    public int? ProductCategoryId { get; private set; }
    public DateOnly? OfferingsClosureDate { get; private set; }
    public FinanceCodes FinanceCodes { get; private set; }

    public void Rename(string name) => Name = name.AsRequiredDomainText();

    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;

    public void ChangeTransferType(int id)
    {
        if (id <= 0)
            throw new DomainException("TransferTypeId must be greater than zero");

        TransferTypeId = id;
    }

    public void ChangeTransferPort(int id)
    {
        if (id <= 0)
            throw new DomainException("TransferPortId must be greater than zero");

        TransferPortId = id;
    }

    public void ChangeUnitType(int unitTypeId)
    {
        if (unitTypeId <= 0)
            throw new DomainException("UnitTypeId must be greater than zero");

        UnitTypeId = unitTypeId;
    }

    public void ChangeTimeWindow(TimeOnly? from, TimeOnly? to)
    {
        if (from.HasValue != to.HasValue)
            throw new DomainException("TimeFrom and TimeTo must either both be set or both be null");

        if (from.HasValue && to.HasValue && from.Value > to.Value)
            throw new DomainException("TimeFrom cannot be greater than TimeTo");

        TimeFrom = from;
        TimeTo = to;
    }

    public void ClearTimeWindow()
    {
        TimeFrom = null;
        TimeTo = null;
    }

    public void ChangeCategories(int? accountCategoryId, int? productCategoryId)
    {
        AccountCategoryId = accountCategoryId;
        ProductCategoryId = productCategoryId;
    }

    public void ChangeFinanceCodes(FinanceCodes codes) => FinanceCodes = codes;
    public void ChangeOfferingsClosureDate(DateOnly? value) => OfferingsClosureDate = value;
}