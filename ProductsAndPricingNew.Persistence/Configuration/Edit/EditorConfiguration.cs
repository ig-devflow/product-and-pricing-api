using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductsAndPricingNew.Domain.Edit;
using ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;

namespace ProductsAndPricingNew.Persistence.Configuration.Edit;

internal sealed class EditorConfiguration : IEntityTypeConfiguration<Editor>
{
    public void Configure(EntityTypeBuilder<Editor> entity)
    {
        entity.ToTable("Editor", "Edit");

        entity.HasKey(x => x.Id);

        entity.Property(x => x.Id)
            .ValueGeneratedNever();

        entity.Property(x => x.UserName)
            .HasMaxLength(Editor.Rules.UserNameMaxLength)
            .IsRequired();

        entity.Property(x => x.FirstName)
            .HasMaxLength(Editor.Rules.FirstNameMaxLength)
            .IsRequired();

        entity.Property(x => x.LastName)
            .HasMaxLength(Editor.Rules.LastNameMaxLength)
            .IsRequired();

        entity.Property(x => x.Email)
            .HasConversion(Converters.EmailAddress)
            .HasColumnName("Email")
            .HasMaxLength(EmailAddress.Rules.MaxLength)
            .IsRequired();;
        
        entity.HasData(new
        {
            Id = 1,
            UserName = "SYSTEM",
            FirstName = "System",
            LastName = "User",
            Email = "noreply@ecenglish.com"
        });
    }
}