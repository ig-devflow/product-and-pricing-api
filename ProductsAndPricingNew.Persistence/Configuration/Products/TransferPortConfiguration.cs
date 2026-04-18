using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductsAndPricingNew.Domain.Entities.Products;

namespace ProductsAndPricingNew.Persistence.Configuration;

public sealed class TransferPortConfiguration : IEntityTypeConfiguration<TransferPort>
{
    public void Configure(EntityTypeBuilder<TransferPort> b)
    {
        b.ToTable("TransferPort");

        b.HasKey(x => x.Id);
        b.Property(x => x.Id).ValueGeneratedNever();

        b.Property(x => x.Name)
            .HasMaxLength(200)
            .IsRequired();

        b.Property(x => x.TransferPortTypeId).IsRequired();
        b.Property(x => x.IsActive).IsRequired();

        b.Navigation(x => x.Instructions)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        b.Navigation(x => x.Terminals)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        b.OwnsMany(x => x.Instructions, owned =>
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

        b.OwnsMany(x => x.Terminals, owned =>
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

        b.Ignore(x => x.DomainEvents);
    }
}