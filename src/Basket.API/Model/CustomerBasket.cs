namespace Basket.API.Model;

public class CustomerBasket
{
    public string CartId { get; set; }

    public List<BasketItem> Items { get; set; } = [];

    public CustomerBasket() { }

    public CustomerBasket(string customerId)
    {
        CartId = customerId;
    }
}
