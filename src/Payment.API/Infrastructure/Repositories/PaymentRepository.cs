namespace Payment.API.Infrastructure.Repositories;

public class PaymentRepository(PaymentContext context) : IPaymentRepository
{
    private readonly PaymentContext _context = context ?? throw new ArgumentNullException(nameof(context));

    public IUnitOfWork UnitOfWork => _context;

    public Domain.Model.Payment Add(Domain.Model.Payment order)
    {
        return _context.Payments.Add(order).Entity;

    }

    public async Task<Domain.Model.Payment> GetByOrderIdAsync(Guid orderId)
    {
        return await _context.Payments.FirstOrDefaultAsync(p => p.OrderId == orderId);
    }

    public async Task<Domain.Model.Payment> GetByPaymentIntentIdAsync(string paymentIntentId)
    {
        return await _context.Payments.FirstOrDefaultAsync(p => p.PaymentIntentId == paymentIntentId);
    }
}