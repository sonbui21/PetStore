namespace Catalog.API.Infrastructure;

public class CatalogContext(DbContextOptions<CatalogContext> options) : DbContext(options)
{
    public required DbSet<CatalogItem> CatalogItems { get; set; }
    public required DbSet<CatalogCategory> CatalogCategories { get; set; }
    public required DbSet<CatalogItemOption> CatalogItemOptions { get; set; }
    public required DbSet<CatalogItemVariant> CatalogItemVariants { get; set; }
    public required DbSet<CatalogItemVariantOption> CatalogItemVariantOptions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("vector");
        modelBuilder.ApplyConfiguration(new CatalogCategoryEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new CatalogItemEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new CatalogItemOptionEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new CatalogItemVariantEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new CatalogItemVariantOptionEntityTypeConfiguration());

        // Add the outbox table to this context
        //modelBuilder.UseIntegrationEventLogs();
    }
}

