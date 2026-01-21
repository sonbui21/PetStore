namespace Ordering.Infrastructure.EntityConfigurations;

class OrderItemEntityTypeConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> orderItemConfiguration)
    {
        orderItemConfiguration.ToTable("orderItems");

        orderItemConfiguration.Ignore(b => b.DomainEvents);

        orderItemConfiguration.Property<Guid>("OrderId");
    }
}
