namespace Ordering.API.Extensions;

public static class BasketItemExtensions
{
    public static IEnumerable<OrderItemDto> ToOrderItemsDto(this IEnumerable<BasketItem> basketItems)
    {
        foreach (var item in basketItems)
        {
            yield return item.ToOrderItemDto();
        }
    }

    public static OrderItemDto ToOrderItemDto(this BasketItem item)
    {
        return new OrderItemDto()
        {
            ProductId = item.ProductId,
            VariantId = item.VariantId,
            Quantity = item.Quantity,
            Title = item.Title,
            Slug = item.Slug,
            Thumbnail = item.Thumbnail,
            Price = item.Price
        };
    }
}
