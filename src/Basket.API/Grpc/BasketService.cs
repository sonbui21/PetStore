namespace Basket.API.Grpc;

public class BasketService(
    IBasketRepository repository,
    ILogger<BasketService> logger) : Basket.BasketBase
{
    [AllowAnonymous]
    public override async Task<CustomerBasketResponse> GetBasket(GetBasketRequest request, ServerCallContext context)
    {
        var userId = context.GetUserIdentity();

        // ===== GUEST =====
        if (string.IsNullOrEmpty(userId))
        {
            if (string.IsNullOrEmpty(request.CartId))
            {
                return new() { CartId = request.CartId };
            }

            var data = await repository.GetBasketAsync(request.CartId);
            return data is not null ? MapToCustomerBasketResponse(data)
                                    : new() { CartId = request.CartId };
        }

        // ===== LOGGED IN =====
        var userCart = await repository.GetBasketAsync(userId);
        
        if (string.IsNullOrEmpty(request.CartId))
        {
            if (userCart is not null)
            {
                return MapToCustomerBasketResponse(userCart);
            }
            return new() { CartId = userId };
        }

        if (userId == request.CartId)
        {
            if (userCart is not null)
            {
                return MapToCustomerBasketResponse(userCart);
            }
            return new() { CartId = userId };
        }

        var guestCart = await repository.GetBasketAsync(request.CartId);
        
        if (userCart is not null && guestCart is not null)
        {
            var mergedCart = MergeBaskets(userCart, guestCart);
            mergedCart.CartId = userId;
            
            var res = await repository.UpdateBasketAsync(mergedCart);
            if (res is null)
            {
                ThrowBasketDoesNotExist(userId);
            }

            await repository.DeleteBasketAsync(request.CartId);
            return MapToCustomerBasketResponse(res);
        }

        if (userCart is not null)
        {
            return MapToCustomerBasketResponse(userCart);
        }

        if (guestCart is not null)
        {
            var oldGuestId = guestCart.CartId;
            guestCart.CartId = userId;
            var res = await repository.UpdateBasketAsync(guestCart);
            if (res is null)
            {
                ThrowBasketDoesNotExist(userId);
            }

            await repository.DeleteBasketAsync(oldGuestId);
            return MapToCustomerBasketResponse(guestCart);
        }

        return new() { CartId = userId };
    }


    [AllowAnonymous]
    public override async Task<CustomerBasketResponse> UpdateBasket(UpdateBasketRequest request, ServerCallContext context)
    {
        var userId = context.GetUserIdentity();
        if (string.IsNullOrEmpty(userId))
        {
            userId = request.CartId;
        }

        // TODO: xu ly merge guest cart

        if (string.IsNullOrEmpty(userId))
        {
            ThrowNotAuthenticated();
        }

        if (logger.IsEnabled(LogLevel.Debug))
        {
            logger.LogDebug("Begin UpdateBasket call from method {Method} for basket id {Id}", context.Method, userId);
        }

        var customerBasket = MapToCustomerBasket(userId, request);
        var response = await repository.UpdateBasketAsync(customerBasket);
        if (response is null)
        {
            ThrowBasketDoesNotExist(userId);
        }

        return MapToCustomerBasketResponse(response);
    }

    [AllowAnonymous]
    public override async Task<DeleteBasketResponse> DeleteBasket(DeleteBasketRequest request, ServerCallContext context)
    {
        var userId = context.GetUserIdentity();
        if (string.IsNullOrEmpty(userId))
        {
            ThrowNotAuthenticated();
        }

        await repository.DeleteBasketAsync(userId);
        return new();
    }

    private static CustomerBasket MergeBaskets(CustomerBasket userCart, CustomerBasket guestCart)
    {
        var mergedCart = new CustomerBasket
        {
            CartId = userCart.CartId,
            Items = new List<Model.BasketItem>()
        };

        // Dictionary để track items đã merge theo key (ProductId + VariantId)
        var itemMap = new Dictionary<string, Model.BasketItem>();

        // Thêm items từ userCart trước
        foreach (var item in userCart.Items)
        {
            var key = $"{item.ProductId}_{item.VariantId}";
            itemMap[key] = new Model.BasketItem
            {
                Id = item.Id,
                ProductId = item.ProductId,
                VariantId = item.VariantId,
                Quantity = item.Quantity,
                Title = item.Title,
                Slug = item.Slug,
                Thumbnail = item.Thumbnail,
                Price = item.Price,
                AvailableStock = item.AvailableStock,
                VariantOptions = item.VariantOptions?.ToList() ?? new List<Model.VariantOption>()
            };
        }

        // Merge items từ guestCart
        foreach (var item in guestCart.Items)
        {
            var key = $"{item.ProductId}_{item.VariantId}";
            if (itemMap.TryGetValue(key, out var existingItem))
            {
                // Nếu đã tồn tại, cộng số lượng
                existingItem.Quantity += item.Quantity;
            }
            else
            {
                // Nếu chưa tồn tại, thêm mới
                itemMap[key] = new Model.BasketItem
                {
                    Id = item.Id,
                    ProductId = item.ProductId,
                    VariantId = item.VariantId,
                    Quantity = item.Quantity,
                    Title = item.Title,
                    Slug = item.Slug,
                    Thumbnail = item.Thumbnail,
                    Price = item.Price,
                    AvailableStock = item.AvailableStock,
                    VariantOptions = item.VariantOptions?.ToList() ?? new List<Model.VariantOption>()
                };
            }
        }

        // Chuyển từ dictionary sang list
        mergedCart.Items = itemMap.Values.ToList();
        return mergedCart;
    }

    [DoesNotReturn]
    private static void ThrowNotAuthenticated() => throw new RpcException(new Status(StatusCode.Unauthenticated, "The caller is not authenticated."));

    [DoesNotReturn]
    private static void ThrowBasketDoesNotExist(string userId) => throw new RpcException(new Status(StatusCode.NotFound, $"Basket with buyer id {userId} does not exist"));

    private static CustomerBasketResponse MapToCustomerBasketResponse(CustomerBasket customerBasket)
    {
        var response = new CustomerBasketResponse()
        {
            CartId = customerBasket.CartId,
        };

        foreach (var item in customerBasket.Items)
        {
            var basketItem = new BasketItem()
            {
                Id = item.Id,
                ProductId = item.ProductId,
                VariantId = item.VariantId,
                Quantity = item.Quantity,

                Title = item.Title,
                Slug = item.Slug,
                Thumbnail = item.Thumbnail,

                Price = item.Price,
                AvailableStock = item.AvailableStock,
            };

            foreach (var option in item.VariantOptions)
            {
                basketItem.VariantOptions.Add(new VariantOption()
                {
                    Name = option.Name,
                    Value = option.Value,
                });
            }

            response.Items.Add(basketItem);
        }

        return response;
    }

    private static CustomerBasket MapToCustomerBasket(string id, UpdateBasketRequest customerBasketRequest)
    {
        var response = new CustomerBasket
        {
            CartId = id,
        };

        foreach (var item in customerBasketRequest.Items)
        {
            var basketItem = new Model.BasketItem()
            {
                Id = item.Id,
                ProductId = item.ProductId,
                VariantId = item.VariantId,
                Quantity = item.Quantity,

                Title = item.Title,
                Slug = item.Slug,
                Thumbnail = item.Thumbnail,

                VariantOptions = [],
                Price = item.Price,
                AvailableStock = item.AvailableStock,
            };

            foreach (var option in item.VariantOptions)
            {
                basketItem.VariantOptions.Add(new()
                {
                    Name = option.Name,
                    Value = option.Value,
                });
            }

            response.Items.Add(basketItem);

        }

        return response;
    }
}
