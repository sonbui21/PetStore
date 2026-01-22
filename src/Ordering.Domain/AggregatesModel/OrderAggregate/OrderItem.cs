namespace Ordering.Domain.AggregatesModel.OrderAggregate;

public class OrderItem : Entity
{
    public Guid ProductId { get; set; }
    public Guid VariantId { get; set; }
    public int Quantity { get; set; }

    public string Title { get; set; }
    public string Slug { get; set; }
    public string Thumbnail { get; set; }
    public decimal Price { get; set; }

    public string VariantOptions { get; set; }

    protected OrderItem() { }

    public OrderItem(Guid productId, Guid variantId, string title, string slug, string thumbnail, decimal price, string variantOptions, int quantity = 1)
    {
        if (quantity <= 0)
        {
            throw new OrderingDomainException("Invalid number of units");
        }

        ProductId = productId;
        VariantId = variantId;
        Quantity = quantity;

        Title = title;
        Slug = slug;
        Thumbnail = thumbnail;
        Price = price;

        VariantOptions = variantOptions;
    }

    public void AddQuantity(int quantity)
    {
        if (quantity < 0)
        {
            throw new OrderingDomainException("Invalid units");
        }

        Quantity += quantity;
    }
}

