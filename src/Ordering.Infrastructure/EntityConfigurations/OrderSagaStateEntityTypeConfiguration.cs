namespace Ordering.Infrastructure.EntityConfigurations;

class OrderSagaStateEntityTypeConfiguration : IEntityTypeConfiguration<OrderSagaState>
{
    public void Configure(EntityTypeBuilder<OrderSagaState> builder)
    {
        builder.ToTable("ordersagas");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.OrderId)
            .IsRequired();

        builder.Property(s => s.CurrentStep)
            .HasConversion<string>()
            .HasMaxLength(40)
            .IsRequired();

        builder.Property(s => s.CreatedAt)
            .IsRequired();

        builder.Property(s => s.UpdatedAt)
            .IsRequired();

        builder.HasIndex(s => s.OrderId)
            .IsUnique();
    }
}
