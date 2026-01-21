namespace Ordering.API.Application.Queries;

public record OrderItem
{
    public Guid ProductId { get; set; }
    public Guid VariantId { get; set; }
    public int Quantity { get; set; }

    public string Title { get; set; }
    public string Slug { get; set; }
    public string Thumbnail { get; set; }
    public decimal Price { get; set; }
}

public record Order
{
    public Guid OrderId { get; init; }
    public DateTime Date { get; init; }
    public string Status { get; init; }
    public string Description { get; init; }
    public string Street { get; init; }
    public string City { get; init; }
    public string State { get; init; }
    public string Zipcode { get; init; }
    public string Country { get; init; }
    public List<OrderItem> OrderItems { get; set; }
    public decimal Total { get; set; }
}

public record OrderSummary
{
    public Guid OrderId { get; init; }
    public DateTime Date { get; init; }
    public string Status { get; init; }
    public double Total { get; init; }
}

public record CardType
{
    public int Id { get; init; }
    public string Name { get; init; }
}

