namespace Basket.API.Model;

public class CustomerBasket
{
    public string CartId { get; set; }

    public List<BasketItem> Items { get; set; } = [];

    public string CurrencyCode { get; set; }
    public string Total { get; set; }
    public string SubTotal { get; set; }
    public string TaxTotal { get; set; }
    public string TotalQuantity { get; set; }

    public string Email { get; set; }
    public Address ShippingAddress { get; set; }
    public Address BillingAddress { get; set; }
    public ShippingMethod ShippingMethods { get; set; }
    public string CurrentStep { get; set; }

    public CustomerBasket() { }

    public CustomerBasket(string customerId)
    {
        CartId = customerId;

        CurrencyCode = "USD";
        Total = "0";
        SubTotal = "0";
        TaxTotal = "0";
        TotalQuantity = "0";

        Email = "";

        CurrentStep = "address";
    }
}

public class Address
{
    public string Id { get; set; }
    public string CustomerId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Phone { get; set; }
    public string Company { get; set; }
    public string Address1 { get; set; }
    public string Address2 { get; set; }
    public string City { get; set; }
    public string CountryCode { get; set; }
    public string Province { get; set; }
    public string PostalCode { get; set; }
}

public class ShippingMethod
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Amount { get; set; }
}