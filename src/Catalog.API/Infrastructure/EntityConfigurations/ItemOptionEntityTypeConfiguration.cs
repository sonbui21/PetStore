namespace Catalog.API.Infrastructure.EntityConfigurations;

class ItemOptionEntityTypeConfiguration : IEntityTypeConfiguration<ItemOption>
{
    public void Configure(EntityTypeBuilder<ItemOption> builder)
    {
        builder.ToTable("ItemOption");
    }
}
