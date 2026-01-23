namespace Ordering.API.Application.Queries;

public class OrderQueries(OrderingContext context) : IOrderQueries
{
    public async Task<Order> GetOrderAsync(Guid id)
    {
        var order = await context.Orders
            .Include(o => o.OrderItems)
            .FirstOrDefaultAsync(o => o.Id == id) ?? throw new KeyNotFoundException();

        return new Order
        {
            OrderId = order.Id,
            Date = order.OrderDate,
            Description = order.Description,
            City = order.Address.City,
            Country = order.Address.Country,
            State = order.Address.State,
            Street = order.Address.Street,
            Zipcode = order.Address.ZipCode,
            Status = order.OrderStatus.ToString(),
            Total = order.GetTotal(),
            OrderItems = [.. order.OrderItems.Select(oi => new OrderItem
            {
                ProductId = oi.ProductId,
                VariantId = oi.VariantId,
                Quantity = oi.Quantity,
                Title = oi.Title,
                Slug = oi.Slug,
                Thumbnail = oi.Thumbnail,
                Price = oi.Price,
                VariantOptions = oi.VariantOptions
            })]
        };
    }

    public async Task<ResultList<OrderSummary>> GetOrdersFromUserAsync(string userId)
    {
        var orders = await context.Orders
            .Where(o => o.Buyer.IdentityGuid == userId)
            .Select(o => new OrderSummary
            {
                OrderId = o.Id,
                Date = o.OrderDate,
                Status = o.OrderStatus.ToString(),
                Total = (double)o.OrderItems.Sum(oi => oi.Price * oi.Quantity),
                OrderItems = o.OrderItems.Select(oi => new OrderItem
                {
                    ProductId = oi.ProductId,
                    VariantId = oi.VariantId,
                    Quantity = oi.Quantity,
                    Title = oi.Title,
                    Slug = oi.Slug,
                    Thumbnail = oi.Thumbnail,
                    Price = oi.Price,
                    VariantOptions = oi.VariantOptions

                }).ToList()
            })
            .OrderByDescending(o => o.Date)
            .ToResultListAsync();


        return orders;
    }

    public async Task<IEnumerable<CardType>> GetCardTypesAsync() =>
        await context.CardTypes.Select(c => new CardType { Id = c.Id, Name = c.Name }).ToListAsync();
}
