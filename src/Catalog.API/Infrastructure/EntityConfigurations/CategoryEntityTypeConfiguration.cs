namespace Catalog.API.Infrastructure.EntityConfigurations;

class CategoryEntityTypeConfiguration: IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Category");
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
