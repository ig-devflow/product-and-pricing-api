using ProductsAndPricingNew.Domain.Base;
using ProductsAndPricingNew.Domain.Common.Errors;
using ProductsAndPricingNew.Domain.Common.Extensions;

namespace ProductsAndPricingNew.Domain.Entities.Products;

public sealed class Accommodation : AggregateRoot<int>
{
    public string Name { get; private set; } = null!;
    public bool IsActive { get; private set; }
    public int AccommodationTypeId { get; private set; }
    public int MinimumStayInWeeks { get; private set; }
    public int MinimumAge { get; private set; }
    public int? MaximumAge { get; private set; }
    public bool IsCommitted { get; private set; }
    public bool IsNonCommitted { get; private set; }

    private Accommodation() { }

    public Accommodation(int id, string name, int accommodationTypeId)
    {
        Id = id;
        AccommodationTypeId = accommodationTypeId;
        IsActive = true;

        Rename(name);
    }

    public void Rename(string name) => Name = name.AsRequiredDomainText();

    public void Activate()
    {
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public void ChangeAccommodationType(int id)
    {
        AccommodationTypeId = id;
    }

    public void ChangeAgeLimits(int minimumAge, int? maximumAge)
    {
        MinimumAge = minimumAge;
        MaximumAge = maximumAge;
    }

    public void ChangeMinimumStay(int weeks)
    {
        MinimumStayInWeeks = weeks;
    }

    public void SetCommitment(bool committed, bool nonCommitted)
    {
        IsCommitted = committed;
        IsNonCommitted = nonCommitted;
    }
}