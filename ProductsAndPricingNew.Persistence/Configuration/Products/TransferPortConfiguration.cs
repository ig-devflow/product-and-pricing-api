using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductsAndPricingNew.Domain.Entities.Products;

namespace ProductsAndPricingNew.Persistence.Configuration.Products;

internal sealed class TransferPortConfiguration : IEntityTypeConfiguration<TransferPort>
{
    public void Configure(EntityTypeBuilder<TransferPort> entity)
    {
        entity.ToTable("TransferPort", "Product");

        entity.HasKey(x => x.Id);
        entity.Property(x => x.Id).ValueGeneratedOnAdd();

        entity.Property(x => x.Name)
            .HasMaxLength(100)
            .IsRequired();

        entity.Property(x => x.TransferPortTypeId).IsRequired();
        entity.Property(x => x.IsActive).IsRequired();

        entity.Navigation(x => x.Instructions)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        entity.Navigation(x => x.Terminals)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        entity.OwnsMany(x => x.Instructions, owned =>
        {
            owned.ToTable("TransferPortInstruction");

            owned.WithOwner().HasForeignKey("TransferPortId");

            owned.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            owned.HasKey(x => x.Id);

            owned.Property(x => x.DivisionId).IsRequired();

            owned.Property(x => x.Instructions)
                .HasMaxLength(4000)
                .IsRequired();

            owned.Property(x => x.IsDeleted).IsRequired();

            owned.HasIndex("TransferPortId", nameof(TransferPortInstruction.DivisionId))
                .IsUnique();
        });

        entity.OwnsMany(x => x.Terminals, owned =>
        {
            owned.ToTable("TransferPortTerminal");

            owned.WithOwner().HasForeignKey("TransferPortId");

            owned.Property<int>("Id")
                .ValueGeneratedOnAdd();

            owned.HasKey("Id");

            owned.Property(x => x.Number).IsRequired();

            owned.Property(x => x.Name)
                .HasMaxLength(200)
                .IsRequired();

            owned.Property(x => x.Order).IsRequired();
            owned.Property(x => x.IsDeleted).IsRequired();

            owned.HasIndex("TransferPortId", nameof(TransferPortTerminal.Number))
                .IsUnique();
        });

        entity.ConfigureAuditMetadata(x => x.AuditMetadata);
        entity.Property(x => x.IsDeleted)
            .HasDefaultValue(false)
            .IsRequired();

        entity.Property(x => x.Version).IsRowVersion();

        entity.Ignore(x => x.DomainEvents);
    }
}