using ProductsAndPricingNew.Domain.Base;

namespace ProductsAndPricingNew.Domain.Entities.Products;

public sealed class AccommodationRoom : AggregateRoot<int>, IProductDefinition
{
    public int AccommodationId { get; private set; }
    public int DivisionId { get; private set; }
    public int UnitTypeId { get; private set; }
    public string Name { get; private set; } = null!;
    public bool IsActive { get; private set; }
    public bool OccupyRoom { get; private set; }
    public int RoomTypeId { get; private set; }
    public int BoardTypeId { get; private set; }
    public int? BathroomTypeId { get; private set; }
    public int RoomGradeId { get; private set; }
    public int? AccountCategoryId { get; private set; }
    public int? ProductCategoryId { get; private set; }
    public DateOnly? OfferingsClosureDate { get; private set; }
    public FinanceCodes FinanceCodes { get; private set; }
    
    private AccommodationRoom() { }

    public AccommodationRoom(
        int id,
        int accommodationId,
        int divisionId,
        int unitTypeId,
        string name,
        int roomTypeId,
        int boardTypeId,
        int roomGradeId)
    {
        Id = id;
        AccommodationId = accommodationId;
        DivisionId = divisionId;
        UnitTypeId = unitTypeId;
        RoomTypeId = roomTypeId;
        BoardTypeId = boardTypeId;
        RoomGradeId = roomGradeId;
        IsActive = true;
        FinanceCodes = new FinanceCodes(null, null);

        Rename(name);
    }
    
    public void Rename(string name)
    {
        Name = name.Trim();
    }

    public void Activate()
    {
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public void ChangeUnitType(int unitTypeId) => UnitTypeId = unitTypeId;
    
    public void SetOccupyRoom(bool value)
    {
        OccupyRoom = value;
    }

    public void ChangeRoomType(int id)
    {
        RoomTypeId = id;
    }

    public void ChangeBoardType(int id)
    {
        BoardTypeId = id;
    }

    public void ChangeBathroomType(int? id)
    {
        BathroomTypeId = id;
    }

    public void ChangeRoomGrade(int id)
    {
        RoomGradeId = id;
    }

    public void ChangeCategories(int? accountCategoryId, int? productCategoryId)
    {
        AccountCategoryId = accountCategoryId;
        ProductCategoryId = productCategoryId;
    }

    public void ChangeFinanceCodes(FinanceCodes codes)
    {
        FinanceCodes = codes;
    }

    public void ChangeOfferingsClosureDate(DateOnly? value)
    {
        OfferingsClosureDate = value;
    }
}
