namespace Basket.API.Repositories;

public class RedisBasketRepository(ILogger<RedisBasketRepository> logger, IConnectionMultiplexer redis) : IBasketRepository
{
    private readonly IDatabase _database = redis.GetDatabase();

    // implementation:

    // - /basket/{id} "string" per unique basket
    private static readonly RedisKey BasketKeyPrefix = "/basket/"u8.ToArray();
    // note on UTF8 here: library limitation (to be fixed) - prefixes are more efficient as blobs

    private static RedisKey GetBasketKey(string userId) => BasketKeyPrefix.Append(userId);

    public async Task<bool> DeleteBasketAsync(string id)
    {
        using var data = await _database.StringGetLeaseAsync(GetBasketKey(id));
        if (data is null || data.Length == 0)
        {
            return false;
        }

        var basket = JsonSerializer.Deserialize(data.Span, BasketSerializationContext.Default.CustomerBasketModel);
        var newBasket = new CustomerBasketModel
        {
            BasketId = basket.BasketId,
            ShippingAddress = basket.ShippingAddress,
            PaymentCollection = basket.PaymentCollection
        };

        var json = JsonSerializer.SerializeToUtf8Bytes(newBasket, BasketSerializationContext.Default.CustomerBasketModel);
        return await _database.StringSetAsync(GetBasketKey(basket.BasketId), json);
    }

    public async Task<CustomerBasketModel> GetBasketAsync(string customerId)
    {
        using var data = await _database.StringGetLeaseAsync(GetBasketKey(customerId));

        if (data is null || data.Length == 0)
        {
            return null;
        }

        return JsonSerializer.Deserialize(data.Span, BasketSerializationContext.Default.CustomerBasketModel);
    }

    public async Task<CustomerBasketModel> UpdateBasketAsync(CustomerBasketModel basket)
    {
        var json = JsonSerializer.SerializeToUtf8Bytes(basket, BasketSerializationContext.Default.CustomerBasketModel);
        var created = await _database.StringSetAsync(GetBasketKey(basket.BasketId), json);

        if (!created)
        {
            logger.LogInformation("Problem occurred persisting the item.");
            return null;
        }


        logger.LogInformation("Basket item persisted successfully.");
        return await GetBasketAsync(basket.BasketId);
    }
}

[JsonSerializable(typeof(CustomerBasketModel))]
[JsonSourceGenerationOptions(PropertyNameCaseInsensitive = true)]
public partial class BasketSerializationContext : JsonSerializerContext
{

}

