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
                return MapToCustomerBasketResponse(new CustomerBasket(request.CartId));
            }

            var data = await repository.GetBasketAsync(request.CartId);
            return data is not null ? MapToCustomerBasketResponse(data)
                                    : MapToCustomerBasketResponse(new CustomerBasket(request.CartId));
        }

        // ===== LOGGED IN =====
        var userCart = await repository.GetBasketAsync(userId);

        if (string.IsNullOrEmpty(request.CartId))
        {
            if (userCart is not null)
            {
                return MapToCustomerBasketResponse(userCart);
            }
            return MapToCustomerBasketResponse(new CustomerBasket(userId));
        }

        if (userId == request.CartId)
        {
            if (userCart is not null)
            {
                return MapToCustomerBasketResponse(userCart);
            }
            return MapToCustomerBasketResponse(new CustomerBasket(userId));
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

        return MapToCustomerBasketResponse(new Model.CustomerBasket(userId));
    }


    [AllowAnonymous]
    public override async Task<CustomerBasketResponse> UpdateBasket(UpdateBasketRequest request, ServerCallContext context)
    {
        var userId = context.GetUserIdentity();
        if (string.IsNullOrEmpty(userId))
        {
            userId = request.CartId;
        }

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

    private static CustomerBasket MergeBaskets(CustomerBasket userCart, CustomerBasket guestCart)
    {
        var mergedCart = new CustomerBasket
        {
            CartId = userCart.CartId,
            Items = [],

            CurrencyCode = userCart.CurrencyCode,
            Total = userCart.Total,
            SubTotal = userCart.SubTotal,
            TaxTotal = userCart.TaxTotal,
            TotalQuantity = userCart.TotalQuantity,

            Email = userCart.Email,

            PaymentCollection = userCart.PaymentCollection,
            CurrentStep = userCart.CurrentStep,
        };

        if (userCart.ShippingAddress is not null)
        {
            mergedCart.ShippingAddress = new Model.Address
            {
                Id = userCart.ShippingAddress.Id,
                CustomerId = userCart.ShippingAddress.CustomerId,
                FirstName = userCart.ShippingAddress.FirstName,
                LastName = userCart.ShippingAddress.LastName,
                Phone = userCart.ShippingAddress.Phone,
                Company = userCart.ShippingAddress.Company,
                Address1 = userCart.ShippingAddress.Address1,
                Address2 = userCart.ShippingAddress.Address2,
                City = userCart.ShippingAddress.City,
                CountryCode = userCart.ShippingAddress.CountryCode,
                Province = userCart.ShippingAddress.Province,
                PostalCode = userCart.ShippingAddress.PostalCode
            };
        }

        if (userCart.BillingAddress is not null)
        {
            mergedCart.ShippingAddress = new Model.Address
            {
                Id = userCart.ShippingAddress.Id,
                CustomerId = userCart.ShippingAddress.CustomerId,
                FirstName = userCart.ShippingAddress.FirstName,
                LastName = userCart.ShippingAddress.LastName,
                Phone = userCart.ShippingAddress.Phone,
                Company = userCart.ShippingAddress.Company,
                Address1 = userCart.ShippingAddress.Address1,
                Address2 = userCart.ShippingAddress.Address2,
                City = userCart.ShippingAddress.City,
                CountryCode = userCart.ShippingAddress.CountryCode,
                Province = userCart.ShippingAddress.Province,
                PostalCode = userCart.ShippingAddress.PostalCode
            };
        }

        if (userCart.ShippingMethods is not null)
        {
            mergedCart.ShippingMethods = new Model.ShippingMethod
            {
                Id = userCart.ShippingMethods.Id,
                Name = userCart.ShippingMethods.Name,
                Description = userCart.ShippingMethods.Description,
                Amount = userCart.ShippingMethods.Amount,
            };
        }

        var itemMap = new Dictionary<string, Model.BasketItem>();

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

        mergedCart.Items = [.. itemMap.Values];
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

            CurrencyCode = customerBasket.CurrencyCode,
            Total = customerBasket.Total,
            SubTotal = customerBasket.SubTotal,
            TaxTotal = customerBasket.TaxTotal,
            TotalQuantity = customerBasket.TotalQuantity,

            Email = customerBasket.Email,
 
            PaymentCollection = customerBasket.PaymentCollection,
            CurrentStep = customerBasket.CurrentStep,
        };

        if (customerBasket.ShippingAddress is not null)
        {
            response.ShippingAddress = new Address
            {
                Id = customerBasket.ShippingAddress.Id,
                CustomerId = customerBasket.ShippingAddress.CustomerId,
                FirstName = customerBasket.ShippingAddress.FirstName,
                LastName = customerBasket.ShippingAddress.LastName,
                Phone = customerBasket.ShippingAddress.Phone,
                Company = customerBasket.ShippingAddress.Company,
                Address1 = customerBasket.ShippingAddress.Address1,
                Address2 = customerBasket.ShippingAddress.Address2,
                City = customerBasket.ShippingAddress.City,
                CountryCode = customerBasket.ShippingAddress.CountryCode,
                Province = customerBasket.ShippingAddress.Province,
                PostalCode = customerBasket.ShippingAddress.PostalCode
            };
        }

        if (customerBasket.BillingAddress is not null)
        {
            response.BillingAddress = new Address
            {
                Id = customerBasket.BillingAddress.Id,
                CustomerId = customerBasket.BillingAddress.CustomerId,
                FirstName = customerBasket.BillingAddress.FirstName,
                LastName = customerBasket.BillingAddress.LastName,
                Phone = customerBasket.BillingAddress.Phone,
                Company = customerBasket.BillingAddress.Company,
                Address1 = customerBasket.BillingAddress.Address1,
                Address2 = customerBasket.BillingAddress.Address2,
                City = customerBasket.BillingAddress.City,
                CountryCode = customerBasket.BillingAddress.CountryCode,
                Province = customerBasket.BillingAddress.Province,
                PostalCode = customerBasket.BillingAddress.PostalCode
            };
        }

        if (customerBasket.ShippingMethods is not null)
        {
            response.ShippingMethods = new ShippingMethod
            {
                Id = customerBasket.ShippingMethods.Id,
                Name = customerBasket.ShippingMethods.Name,
                Description = customerBasket.ShippingMethods.Description,
                Amount = customerBasket.ShippingMethods.Amount,
            };
        }

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

    private static CustomerBasket MapToCustomerBasket(string id, UpdateBasketRequest customerBasket)
    {
        var response = new CustomerBasket
        {
            CartId = id,
            CurrencyCode = customerBasket.CurrencyCode,
            Total = customerBasket.Total,
            SubTotal = customerBasket.SubTotal,
            TaxTotal = customerBasket.TaxTotal,
            TotalQuantity = customerBasket.TotalQuantity,

            Email = customerBasket.Email,

            PaymentCollection = customerBasket.PaymentCollection,
            CurrentStep = customerBasket.CurrentStep,
        };

        if (customerBasket.ShippingAddress is not null)
        {
            response.ShippingAddress = new Model.Address
            {
                Id = customerBasket.ShippingAddress.Id,
                CustomerId = customerBasket.ShippingAddress.CustomerId,
                FirstName = customerBasket.ShippingAddress.FirstName,
                LastName = customerBasket.ShippingAddress.LastName,
                Phone = customerBasket.ShippingAddress.Phone,
                Company = customerBasket.ShippingAddress.Company,
                Address1 = customerBasket.ShippingAddress.Address1,
                Address2 = customerBasket.ShippingAddress.Address2,
                City = customerBasket.ShippingAddress.City,
                CountryCode = customerBasket.ShippingAddress.CountryCode,
                Province = customerBasket.ShippingAddress.Province,
                PostalCode = customerBasket.ShippingAddress.PostalCode
            };
        }

        if (customerBasket.BillingAddress is not null)
        {
            response.BillingAddress = new Model.Address
            {
                Id = customerBasket.BillingAddress.Id,
                CustomerId = customerBasket.BillingAddress.CustomerId,
                FirstName = customerBasket.BillingAddress.FirstName,
                LastName = customerBasket.BillingAddress.LastName,
                Phone = customerBasket.BillingAddress.Phone,
                Company = customerBasket.BillingAddress.Company,
                Address1 = customerBasket.BillingAddress.Address1,
                Address2 = customerBasket.BillingAddress.Address2,
                City = customerBasket.BillingAddress.City,
                CountryCode = customerBasket.BillingAddress.CountryCode,
                Province = customerBasket.BillingAddress.Province,
                PostalCode = customerBasket.BillingAddress.PostalCode
            };
        }

        if (customerBasket.ShippingMethods is not null)
        {
            response.ShippingMethods = new Model.ShippingMethod
            {
                Id = customerBasket.ShippingMethods.Id,
                Name = customerBasket.ShippingMethods.Name,
                Description = customerBasket.ShippingMethods.Description,
                Amount = customerBasket.ShippingMethods.Amount,
            };
        }

        foreach (var item in customerBasket.Items)
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
