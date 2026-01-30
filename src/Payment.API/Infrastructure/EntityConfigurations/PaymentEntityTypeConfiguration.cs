namespace Payment.API.Infrastructure.EntityConfigurations;

public class PaymentEntityTypeConfiguration : IEntityTypeConfiguration<Domain.Model.Payment>
{

    public void Configure(EntityTypeBuilder<Domain.Model.Payment> paymentConfiguration)
    {
        paymentConfiguration.ToTable("payments");

        paymentConfiguration.Ignore(b => b.DomainEvents);

        paymentConfiguration.Property(p => p.PaymentIntentId)
           .IsRequired();

        paymentConfiguration.Property(p => p.OrderId)
            .IsRequired();

        paymentConfiguration.Property(p => p.Amount)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        paymentConfiguration.HasIndex(p => p.OrderId)
            .IsUnique();
    }
}
