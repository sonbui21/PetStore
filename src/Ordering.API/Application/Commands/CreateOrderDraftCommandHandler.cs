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
            order.AddOrderItem(item.ProductId, item.ProductName, item.UnitPrice, item.Discount, item.PictureUrl, item.Units);
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
                Discount = oi.Discount,
                ProductId = oi.ProductId,
                UnitPrice = oi.UnitPrice,
                PictureUrl = oi.PictureUrl,
                Units = oi.Units,
                ProductName = oi.ProductName
            }),
            Total = order.GetTotal()
        };
    }
}

public record OrderItemDto
{
    public int ProductId { get; init; }

    public string ProductName { get; init; }

    public decimal UnitPrice { get; init; }

    public decimal Discount { get; init; }

    public int Units { get; init; }

    public string PictureUrl { get; init; }
}
