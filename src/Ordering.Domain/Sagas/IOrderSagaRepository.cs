namespace Ordering.Domain.Sagas;

public interface IOrderSagaRepository : IRepository<OrderSagaState>
{
    OrderSagaState Add(OrderSagaState sagaState);

    void Update(OrderSagaState sagaState);

    Task<OrderSagaState> GetAsync(Guid orderId);
}
