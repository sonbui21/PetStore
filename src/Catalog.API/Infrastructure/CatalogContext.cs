namespace Catalog.API.Infrastructure;

public class CatalogContext(DbContextOptions<CatalogContext> options) : DbContext(options)
{
    public required DbSet<CatalogItem> CatalogItems { get; set; }
    public required DbSet<Category> Categories { get; set; }
    public required DbSet<ItemOption> ItemOptions { get; set; }
    public required DbSet<ItemVariant> ItemVariants { get; set; }
    public required DbSet<ItemVariantOption> ItemVariantOptions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CatalogItemEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new CategoryEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new ItemOptionEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new ItemVariantEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new ItemVariantOptionEntityTypeConfiguration());

        modelBuilder.UseIntegrationEventLogs();
    }
}

