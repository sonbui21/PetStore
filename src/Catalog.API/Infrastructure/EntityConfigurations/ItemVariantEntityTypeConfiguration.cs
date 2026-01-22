namespace Catalog.API.Infrastructure.EntityConfigurations;

class ItemVariantEntityTypeConfiguration : IEntityTypeConfiguration<ItemVariant>
{
    public void Configure(EntityTypeBuilder<ItemVariant> builder)
    {
        builder.ToTable("ItemVariant");

        builder.HasKey(v => v.Id);

        builder.HasIndex(v => new { v.CatalogItemId, v.Title })
            .IsUnique();

        builder.Property(v => v.Title)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(v => v.Price)
            .HasColumnType("decimal(18,2)");

        builder.Property(v => v.AvailableStock)
            .IsRequired();

        builder.HasOne(v => v.CatalogItem)
            .WithMany(i => i.ItemVariants)
            .HasForeignKey(v => v.CatalogItemId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(v => v.Options)
            .WithOne(o => o.ItemVariant)
            .HasForeignKey(o => o.ItemVariantId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(v => v.CatalogItemId);
    }
}

