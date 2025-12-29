namespace Catalog.API.Infrastructure.EntityConfigurations;

class ItemVariantOptionEntityTypeConfiguration : IEntityTypeConfiguration<ItemVariantOption>
{
    public void Configure(EntityTypeBuilder<ItemVariantOption> builder)
    {
        builder.ToTable("ItemVariantOption");

        builder.HasKey(o => o.Id);

        builder.Property(o => o.Name)
            .IsRequired()
            .HasMaxLength(64);

        builder.Property(o => o.Value)
            .IsRequired()
            .HasMaxLength(128);

        builder.HasIndex(o => new { o.ItemVariantId, o.Name });

        builder.HasOne(o => o.ItemVariant)
            .WithMany(v => v.Options)
            .HasForeignKey(o => o.ItemVariantId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
