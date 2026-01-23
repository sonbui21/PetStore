namespace Basket.API.Model;

public class CustomerBasketModel
{
    public string BasketId { get; set; }
    public List<BasketItemModel> Items { get; set; } = [];
    public AddressModel ShippingAddress { get; set; }
    public string PaymentCollection { get; set; }

    public CustomerBasketModel() { }

    public CustomerBasketModel(string customerId)
    {
        BasketId = customerId;
        PaymentCollection = "";
    }
}

public class AddressModel
{
    public string Name { get; set; }
    public string Phone { get; set; }
    public string Street { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Country { get; set; }
    public string ZipCode { get; set; }
}
