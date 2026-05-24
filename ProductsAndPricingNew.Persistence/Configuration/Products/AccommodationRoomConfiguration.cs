using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductsAndPricingNew.Domain.Entities.Products;

namespace ProductsAndPricingNew.Persistence.Configuration.Products;

internal sealed class AccommodationRoomConfiguration : IEntityTypeConfiguration<AccommodationRoom>
{
    public void Configure(EntityTypeBuilder<AccommodationRoom> entity)
    {
        entity.ToTable("AccommodationRoom", "Product");

        entity.HasKey(x => x.Id);
        entity.Property(x => x.Id).ValueGeneratedOnAdd();

        entity.Property(x => x.Name)
            .HasMaxLength(AccommodationRoom.Rules.NameMaxLength)
            .IsRequired();

        entity.Property(x => x.AccommodationId).IsRequired();
        entity.Property(x => x.DivisionId).IsRequired();
        entity.Property(x => x.UnitTypeId).IsRequired();
        entity.Property(x => x.IsActive).IsRequired();
        entity.Property(x => x.OccupyRoom).IsRequired();

        entity.ComplexProperty(x => x.RoomDetails, rd =>
        {
            rd.Property(x => x.RoomTypeId).HasColumnName("RoomTypeId").IsRequired();
            rd.Property(x => x.BoardTypeId).HasColumnName("BoardTypeId").IsRequired();
            rd.Property(x => x.BathroomTypeId).HasColumnName("BathroomTypeId").IsRequired();
            rd.Property(x => x.RoomGradeId).HasColumnName("RoomGradeId").IsRequired();
        });

        entity.ConfigureProductCategories(x => x.Categories);
        entity.ConfigureFinanceCodes(x => x.FinanceCodes);

        entity.Property(x => x.ClosurePolicy)
            .HasConversion(Converters.OfferingsClosurePolicy)
            .HasColumnName("OfferingsClosureDate");

        entity.HasIndex(x => x.AccommodationId);
        entity.HasIndex(x => new { x.DivisionId, x.Name });

        entity.HasOne<Accommodation>()
            .WithMany()
            .HasForeignKey(x => x.AccommodationId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.ConfigureAuditAndConcurrency();
    }
}