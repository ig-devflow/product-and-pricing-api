using ProductsAndPricingNew.Domain.Common.Primitives;
using ProductsAndPricingNew.Domain.Common.Text;
using ProductsAndPricingNew.Domain.Entities.Products.Definitions;
using ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;
using ProductsAndPricingNew.Domain.UnitOfMeasure;

namespace ProductsAndPricingNew.Domain.Entities.Products;

public sealed class AccommodationRoom : AggregateRoot<int>, IProductDefinition
{
    public int AccommodationId { get; private set; }
    public int DivisionId { get; private set; }
    public string Name { get; private set; } = null!;
    public int UnitTypeId { get; private set; }
    public bool IsActive { get; private set; }
    public bool OccupyRoom { get; private set; }
    public RoomDetails RoomDetails { get; private set; } = RoomDetails.Unassigned;
    public ProductCategories Categories { get; private set; } = ProductCategories.Unassigned;
    public FinanceCodes FinanceCodes { get; private set; } = FinanceCodes.Unassigned;
    public OfferingsClosurePolicy ClosurePolicy { get; private set; } = OfferingsClosurePolicy.Open;

    private AccommodationRoom() { }

    private AccommodationRoom(int accommodationId, int divisionId, int unitTypeId, string name)
    {
        AccommodationId = accommodationId;
        DivisionId = divisionId;
        UnitTypeId = unitTypeId;
        Name = name;
    }

    public void Rename(string name) =>
        Name = name.AsRequiredDomainText(nameof(Name), Rules.NameMaxLength);

    public void WithUnitType(UnitType unitType)
    {
        UnitTypePolicy.EnsureAllowedForProduct(ProductKind.AccommodationRoom, unitType);
        UnitTypeId = unitType.Id;
    }

    public void SetIsActive(bool isActive) =>
        IsActive = isActive;

    public void SetOccupyRoom(bool occupyRoom) =>
        OccupyRoom = occupyRoom;

    public void WithRoomDetails(RoomDetailsDefinition roomDetails) =>
        RoomDetails = RoomDetails.Create(roomDetails);

    public void WithCategories(int accountCategoryId, int productCategoryId) =>
        Categories = ProductCategories.Create(accountCategoryId, productCategoryId);

    public void WithFinanceCodes(string? generalLedgerCode, string? costCentreCode) =>
        FinanceCodes = FinanceCodes.Create(generalLedgerCode, costCentreCode);

    public void WithClosurePolicy(DateOnly? date) =>
        ClosurePolicy = OfferingsClosurePolicy.Create(date);

    public sealed class Builder
    {
        private readonly int _accommodationId;
        private readonly int _divisionId;
        private readonly int _unitTypeId;
        private readonly string _name;

        private bool _isActive;
        private bool _occupyRoom;
        private RoomDetails _roomDetails = RoomDetails.Unassigned;
        private ProductCategories _categories = ProductCategories.Unassigned;
        private FinanceCodes _financeCodes = FinanceCodes.Unassigned;
        private OfferingsClosurePolicy _closurePolicy = OfferingsClosurePolicy.Open;

        public Builder(int accommodationId, int divisionId, UnitType unitType, string name)
        {
            ArgumentNullException.ThrowIfNull(unitType);
            UnitTypePolicy.EnsureAllowedForProduct(ProductKind.AccommodationRoom, unitType);

            _accommodationId = Guard.PositiveId(accommodationId, nameof(AccommodationId));
            _divisionId = Guard.PositiveId(divisionId, nameof(DivisionId));
            _unitTypeId = unitType.Id;
            _name = name.AsRequiredDomainText(nameof(Name), Rules.NameMaxLength);
        }

        public Builder SetIsActive(bool value)
        {
            _isActive = value;
            return this;
        }

        public Builder SetOccupyRoom(bool value)
        {
            _occupyRoom = value;
            return this;
        }

        public Builder WithRoomDetails(RoomDetailsDefinition roomDetails)
        {
            _roomDetails = RoomDetails.Create(roomDetails);
            return this;
        }

        public Builder WithCategories(int accountCategoryId, int productCategoryId)
        {
            _categories = ProductCategories.Create(accountCategoryId, productCategoryId);
            return this;
        }

        public Builder WithFinanceCodes(string? generalLedgerCode, string? costCentreCode)
        {
            _financeCodes = FinanceCodes.Create(generalLedgerCode, costCentreCode);
            return this;
        }

        public Builder WithClosurePolicy(DateOnly? value)
        {
            _closurePolicy = OfferingsClosurePolicy.Create(value);
            return this;
        }

        public AccommodationRoom Build()
        {
            AccommodationRoom accommodationRoom = new(_accommodationId, _divisionId, _unitTypeId, _name)
            {
                IsActive = _isActive,
                OccupyRoom = _occupyRoom,
                RoomDetails = _roomDetails,
                Categories = _categories,
                FinanceCodes = _financeCodes,
                ClosurePolicy = _closurePolicy
            };

            return accommodationRoom;
        }
    }

    public static class Rules
    {
        public const int NameMaxLength = 100;
    }
}
