namespace Basket.API.Model;

public class CustomerBasket
{
    public string BasketId { get; set; }
    public List<BasketItem> Items { get; set; } = [];
    public Address ShippingAddress { get; set; }
    public string PaymentCollection { get; set; }

    public CustomerBasket() { }

    public CustomerBasket(string customerId)
    {
        BasketId = customerId;
        PaymentCollection = "";
    }
}

public class Address
{
    public string Name { get; set; }
    public string Phone { get; set; }
    public string Street { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Country { get; set; }
    public string ZipCode { get; set; }
}
