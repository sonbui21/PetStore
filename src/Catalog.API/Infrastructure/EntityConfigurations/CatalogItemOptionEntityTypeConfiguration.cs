namespace Catalog.API.Infrastructure.EntityConfigurations;

class CatalogItemOptionEntityTypeConfiguration : IEntityTypeConfiguration<CatalogItemOption>
{
    public void Configure(EntityTypeBuilder<CatalogItemOption> builder)
    {
        builder.ToTable("CatalogItemOption");
    }
}
