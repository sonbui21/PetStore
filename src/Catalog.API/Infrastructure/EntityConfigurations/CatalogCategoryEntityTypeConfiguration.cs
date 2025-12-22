namespace Catalog.API.Infrastructure.EntityConfigurations;

class CatalogCategoryEntityTypeConfiguration: IEntityTypeConfiguration<CatalogCategory>
{
    public void Configure(EntityTypeBuilder<CatalogCategory> builder)
    {
        builder.ToTable("CatalogCategory");
        builder
            .Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder
            .Property(c => c.Slug)
            .IsRequired()
            .HasMaxLength(50);

        builder
            .HasIndex(c => c.Index)
            .IsUnique();
    }
}
