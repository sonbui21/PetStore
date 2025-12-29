namespace Catalog.API.Infrastructure.EntityConfigurations;

class CatalogItemEntityTypeConfiguration : IEntityTypeConfiguration<CatalogItem>
{
    public void Configure(EntityTypeBuilder<CatalogItem> builder)
    {
        builder.ToTable("CatalogItem");

        builder
            .Property(ci => ci.Title)
            .IsRequired()
            .HasMaxLength(200);
        builder
            .Property(ci => ci.Slug)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasIndex(ci => ci.Slug).IsUnique();
        builder.HasIndex(ci => ci.Title);

        builder
            .HasMany(ci => ci.Categories)
            .WithMany(c => c.CatalogItems);
        builder
            .HasMany(ci => ci.ItemOptions)
            .WithOne(o => o.CatalogItem)
            .HasForeignKey(o => o.CatalogItemId);
    }
}
