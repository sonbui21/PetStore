using CustomerBasketModel = Basket.API.Model.CustomerBasket;
using AddressModel = Basket.API.Model.Address;
using BasketItemModel = Basket.API.Model.BasketItem;

namespace Basket.API.Grpc;

public class BasketService(
    IBasketRepository repository,
    ILogger<BasketService> logger) : Basket.BasketBase
{
    [AllowAnonymous]
    public override async Task<CustomerBasket> GetBasket(GetBasketRequest request, ServerCallContext context)
    {
        var userId = context.GetUserIdentity();

        // ===== GUEST =====
        if (string.IsNullOrEmpty(userId))
        {
            if (string.IsNullOrEmpty(request.BasketId))
            {
                return MapToCustomerBasketResponse(new CustomerBasketModel(request.BasketId));
            }

            var data = await repository.GetBasketAsync(request.BasketId);
            return data is not null ? MapToCustomerBasketResponse(data)
                                    : MapToCustomerBasketResponse(new CustomerBasketModel(request.BasketId));
        }

        // ===== LOGGED IN =====
        var userCart = await repository.GetBasketAsync(userId);

        if (string.IsNullOrEmpty(request.BasketId))
        {
            if (userCart is not null)
            {
                return MapToCustomerBasketResponse(userCart);
            }
            return MapToCustomerBasketResponse(new CustomerBasketModel(userId));
        }

        if (userId == request.BasketId)
        {
            if (userCart is not null)
            {
                return MapToCustomerBasketResponse(userCart);
            }
            return MapToCustomerBasketResponse(new CustomerBasketModel(userId));
        }

        var guestCart = await repository.GetBasketAsync(request.BasketId);

        if (userCart is not null && guestCart is not null)
        {
            var mergedCart = MergeBaskets(userCart, guestCart);
            mergedCart.BasketId = userId;

            var res = await repository.UpdateBasketAsync(mergedCart);
            if (res is null)
            {
                ThrowBasketDoesNotExist(userId);
            }

            await repository.DeleteBasketAsync(request.BasketId);
            return MapToCustomerBasketResponse(res);
        }

        if (userCart is not null)
        {
            return MapToCustomerBasketResponse(userCart);
        }

        if (guestCart is not null)
        {
            var oldGuestId = guestCart.BasketId;
            guestCart.BasketId = userId;
            var res = await repository.UpdateBasketAsync(guestCart);
            if (res is null)
            {
                ThrowBasketDoesNotExist(userId);
            }

            await repository.DeleteBasketAsync(oldGuestId);
            return MapToCustomerBasketResponse(guestCart);
        }

        return MapToCustomerBasketResponse(new CustomerBasketModel(userId));
    }


    [AllowAnonymous]
    public override async Task<CustomerBasket> UpdateBasket(CustomerBasket request, ServerCallContext context)
    {
        var userId = context.GetUserIdentity();
        if (string.IsNullOrEmpty(userId))
        {
            userId = request.BasketId;
        }

        if (string.IsNullOrEmpty(userId))
        {
            ThrowNotAuthenticated();
        }

        LogExtensions.LogBeginUpdateBasket(logger, context.Method, userId);

        var customerBasket = MapToCustomerBasketModel(userId, request);
        var response = await repository.UpdateBasketAsync(customerBasket);
        if (response is null)
        {
            ThrowBasketDoesNotExist(userId);
        }

        return MapToCustomerBasketResponse(response);
    }

    private static CustomerBasketModel MergeBaskets(CustomerBasketModel userCart, CustomerBasketModel guestCart)
    {
        var mergedCart = new CustomerBasketModel
        {
            BasketId = userCart.BasketId,
            Items = [],
            PaymentCollection = userCart.PaymentCollection,
        };

        if (userCart.ShippingAddress is not null)
        {
            mergedCart.ShippingAddress = new AddressModel
            {
                Name = userCart.ShippingAddress.Name,
                Phone = userCart.ShippingAddress.Phone,
                Street = userCart.ShippingAddress.Street,
                State = userCart.ShippingAddress.State,
                City = userCart.ShippingAddress.Phone,
                Country = userCart.ShippingAddress.Country,
                ZipCode = userCart.ShippingAddress.ZipCode,
            };
        }
        
        var itemMap = new Dictionary<string, BasketItemModel>();
        foreach (var item in userCart.Items)
        {
            var key = $"{item.ProductId}_{item.VariantId}";
            itemMap[key] = new BasketItemModel
            {
                ProductId = item.ProductId,
                VariantId = item.VariantId,
                Quantity = item.Quantity,
                Title = item.Title,
                Slug = item.Slug,
                Thumbnail = item.Thumbnail,
                Price = item.Price,
                AvailableStock = item.AvailableStock,
                VariantOptions = item.VariantOptions
            };
        }

        foreach (var item in guestCart.Items)
        {
            var key = $"{item.ProductId}_{item.VariantId}";
            if (itemMap.TryGetValue(key, out var existingItem))
            {
                existingItem.Quantity += item.Quantity;
            }
            else
            {
                itemMap[key] = new Model.BasketItem
                {
                    ProductId = item.ProductId,
                    VariantId = item.VariantId,
                    Quantity = item.Quantity,
                    Title = item.Title,
                    Slug = item.Slug,
                    Thumbnail = item.Thumbnail,
                    Price = item.Price,
                    AvailableStock = item.AvailableStock,
                    VariantOptions = item.VariantOptions
                };
            }
        }

        mergedCart.Items = [.. itemMap.Values];
        return mergedCart;
    }

    [DoesNotReturn]
    private static void ThrowNotAuthenticated() => throw new RpcException(new Status(StatusCode.Unauthenticated, "The caller is not authenticated."));

    [DoesNotReturn]
    private static void ThrowBasketDoesNotExist(string userId) => throw new RpcException(new Status(StatusCode.NotFound, $"Basket with buyer id {userId} does not exist"));

    private static CustomerBasket MapToCustomerBasketResponse(CustomerBasketModel customerBasket)
    {
        var response = new CustomerBasket()
        {
            BasketId = customerBasket.BasketId,
            PaymentCollection = customerBasket.PaymentCollection,
        };

        if (customerBasket.ShippingAddress is not null)
        {
            response.ShippingAddress = new Address
            {
                Name = customerBasket.ShippingAddress.Name,
                Phone = customerBasket.ShippingAddress.Phone,
                Street = customerBasket.ShippingAddress.Street,
                State = customerBasket.ShippingAddress.State,
                City = customerBasket.ShippingAddress.Phone,
                Country = customerBasket.ShippingAddress.Country,
                ZipCode = customerBasket.ShippingAddress.ZipCode,
            };
        }
        
        foreach (var item in customerBasket.Items)
        {
            var basketItem = new BasketItem()
            {
                ProductId = item.ProductId,
                VariantId = item.VariantId,
                Quantity = item.Quantity,

                Title = item.Title,
                Slug = item.Slug,
                Thumbnail = item.Thumbnail,

                Price = item.Price,
                AvailableStock = item.AvailableStock,
                VariantOptions = item.VariantOptions,
            };

            response.Items.Add(basketItem);
        }

        return response;
    }

    private static CustomerBasketModel MapToCustomerBasketModel(string id, CustomerBasket customerBasket)
    {
        var response = new CustomerBasketModel
        {
            BasketId = id,
            PaymentCollection = customerBasket.PaymentCollection,
        };

        if (customerBasket.ShippingAddress is not null)
        {
            response.ShippingAddress = new AddressModel
            {
                Name = customerBasket.ShippingAddress.Name,
                Phone = customerBasket.ShippingAddress.Phone,
                Street = customerBasket.ShippingAddress.Street,
                State = customerBasket.ShippingAddress.State,
                City = customerBasket.ShippingAddress.Phone,
                Country = customerBasket.ShippingAddress.Country,
                ZipCode = customerBasket.ShippingAddress.ZipCode,
            };
        }

        foreach (var item in customerBasket.Items)
        {
            var basketItem = new BasketItemModel()
            {
                ProductId = item.ProductId,
                VariantId = item.VariantId,
                Quantity = item.Quantity,

                Title = item.Title,
                Slug = item.Slug,
                Thumbnail = item.Thumbnail,

                Price = item.Price,
                AvailableStock = item.AvailableStock,
                VariantOptions = item.VariantOptions,
            };

            response.Items.Add(basketItem);

        }

        return response;
    }
}
