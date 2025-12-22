namespace Catalog.API.Infrastructure.EntityConfigurations;

class CatalogItemVariantEntityTypeConfiguration : IEntityTypeConfiguration<CatalogItemVariant>
{
    public void Configure(EntityTypeBuilder<CatalogItemVariant> builder)
    {
        builder.ToTable("CatalogItemVariant");

        // Primary key
        builder.HasKey(v => v.Id);

        // Required fields
        builder.Property(v => v.Title)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(v => v.CurrencyCode)
            .IsRequired()
            .HasMaxLength(3);

        builder.Property(v => v.Price)
            .HasColumnType("decimal(18,2)");

        // Inventory
        builder.Property(v => v.AvailableStock)
            .IsRequired();

        // Ignore computed property
        builder.Ignore(v => v.AvailableForSale);

        // Relationships
        builder.HasOne(v => v.CatalogItem)
            .WithMany(i => i.CatalogItemVariants)
            .HasForeignKey(v => v.CatalogItemId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(v => v.SelectedOptions)
            .WithOne(o => o.CatalogItemVariant)
            .HasForeignKey(o => o.CatalogItemVariantId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes (performance)
        builder.HasIndex(v => v.CatalogItemId);
    }
}

