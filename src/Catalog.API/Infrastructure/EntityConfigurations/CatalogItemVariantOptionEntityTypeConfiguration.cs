namespace Catalog.API.Infrastructure.EntityConfigurations;

class CatalogItemVariantOptionEntityTypeConfiguration : IEntityTypeConfiguration<CatalogItemVariantOption>
{
    public void Configure(EntityTypeBuilder<CatalogItemVariantOption> builder)
    {
        builder.ToTable("CatalogItemVariantOption");

        builder.HasKey(o => o.Id);

        builder.Property(o => o.Name)
            .IsRequired()
            .HasMaxLength(64);

        builder.Property(o => o.Value)
            .IsRequired()
            .HasMaxLength(128);

        builder.HasIndex(o => new { o.CatalogItemVariantId, o.Name });

        builder.HasOne(o => o.CatalogItemVariant)
            .WithMany(v => v.SelectedOptions)
            .HasForeignKey(o => o.CatalogItemVariantId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
