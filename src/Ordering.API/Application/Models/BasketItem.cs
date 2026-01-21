namespace Ordering.API.Application.Models;

public class BasketItem
{
    public Guid ProductId { get; set; }
    public Guid VariantId { get; set; }
    public int Quantity { get; set; }

    public string Title { get; set; }
    public string Slug { get; set; }
    public string Thumbnail { get; set; }

    public decimal Price { get; set; }
}
