namespace Payment.API.Domain.Model;

public interface IPaymentRepository : IRepository<Payment>
{
    Payment Add(Payment order);
    Task<Payment> GetByOrderIdAsync(Guid orderId);
    Task<Payment> GetByPaymentIntentIdAsync(string paymentIntentId);
}
