
namespace Basket.API.Repositories;

public interface IBasketRepository
{
    Task<CustomerBasketModel> GetBasketAsync(string customerId);
    Task<CustomerBasketModel> UpdateBasketAsync(CustomerBasketModel basket);
    Task<bool> DeleteBasketAsync(string id);
}