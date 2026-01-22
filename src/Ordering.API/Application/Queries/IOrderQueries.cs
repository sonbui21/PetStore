namespace Ordering.API.Application.Queries;

public interface IOrderQueries
{
    Task<Order> GetOrderAsync(Guid id);

    Task<ResultList<OrderSummary>> GetOrdersFromUserAsync(string userId);

    Task<IEnumerable<CardType>> GetCardTypesAsync();
}
