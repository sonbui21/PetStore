using Order = Ordering.Domain.AggregatesModel.OrderAggregate.Order;

namespace Ordering.API.Application.Commands;

// Regular CommandHandler
public class CreateOrderDraftCommandHandler
    : IRequestHandler<CreateOrderDraftCommand, OrderDraftDto>
{
    public Task<OrderDraftDto> Handle(CreateOrderDraftCommand message, CancellationToken cancellationToken)
    {
        var order = Order.NewDraft();
        var orderItems = message.Items.Select(i => i.ToOrderItemDto());
        foreach (var item in orderItems)
        {
            order.AddOrderItem(item.ProductId, item.VariantId, item.Title, item.Slug, item.Thumbnail, item.Price, item.Quantity);
        }

        return Task.FromResult(OrderDraftDto.FromOrder(order));
    }
}

public record OrderDraftDto
{
    public IEnumerable<OrderItemDto> OrderItems { get; init; }
    public decimal Total { get; init; }

    public static OrderDraftDto FromOrder(Order order)
    {
        return new OrderDraftDto()
        {
            OrderItems = order.OrderItems.Select(oi => new OrderItemDto
            {
                ProductId = oi.ProductId,
                VariantId = oi.VariantId,
                Quantity = oi.Quantity,
                Title = oi.Title,
                Slug = oi.Slug,
                Thumbnail = oi.Thumbnail,
                Price = oi.Price,
            }),
            Total = order.GetTotal()
        };
    }
}

public record OrderItemDto
{
    public Guid ProductId { get; set; }
    public Guid VariantId { get; set; }
    public int Quantity { get; set; }

    public string Title { get; set; }
    public string Slug { get; set; }
    public string Thumbnail { get; set; }
    public decimal Price { get; set; }
}
