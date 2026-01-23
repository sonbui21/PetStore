namespace Ordering.Infrastructure.Repositories;

public class OrderSagaRepository(OrderingContext context) : IOrderSagaRepository
{
    private readonly OrderingContext _context = context ?? throw new ArgumentNullException(nameof(context));

    public IUnitOfWork UnitOfWork => _context;

    public OrderSagaState Add(OrderSagaState sagaState)
    {
        return _context.OrderSagas.Add(sagaState).Entity;
    }

    public async Task<OrderSagaState> GetAsync(Guid orderId)
    {
        return await _context.OrderSagas.SingleOrDefaultAsync(s => s.OrderId == orderId);
    }

    public void Update(OrderSagaState sagaState)
    {
        _context.Entry(sagaState).State = EntityState.Modified;
    }
}
