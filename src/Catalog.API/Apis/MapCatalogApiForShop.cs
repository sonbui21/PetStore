namespace Catalog.API.Apis;

public static class CatalogApiForShop
{
    public static IEndpointRouteBuilder MapCatalogApiForShop(this IEndpointRouteBuilder app)
    {
        // RouteGroupBuilder for catalog endpoints
        var api = app.MapGroup("api/shop/catalog");

        // Routes for querying item.
        api.MapGet("/premium-items", GetPremiumItems)
            .WithName("GetPremiumItems")
            .WithSummary("List catalog items for homepage")
            .WithDescription("Get a list of item for homepage in the catalog.")
            .WithTags("Shop/Items");

        api.MapGet("/items/{slug:minlength(1)}", GetItemBySlug)
            .WithName("GetItemBySlug")
            .WithSummary("Get catalog item by slug")
            .WithDescription("Get an item from the catalog")
            .WithTags("Shop/Items");

        api.MapGet("/items/category/{categorySlug:minlength(1)}", GetItemsByCategorySlug)
            .WithName("GetItemsByCategorySlug")
            .WithSummary("List catalog items by categorySlug")
            .WithDescription("Get a list of item by categorySlug in the catalog.")
            .WithTags("Shop/Items");

        api.MapGet("/items/title/{title:minlength(1)}", GetItemsByTitle)
            .WithName("GetItemsByTitle")
            .WithSummary("List catalog items by title")
            .WithDescription("Get a list of item by name in the catalog.")
            .WithTags("Shop/Items");


        // Routes for querying category.
        api.MapGet("/nav-categories", GetCategoriesForNav)
            .WithName("GetCategoriesForNav")
            .WithSummary("List catalog categories for navigation")
            .WithDescription("Get a list of category for navigation in the catalog.")
            .WithTags("Shop/Categories");

        api.MapGet("/search-categories", GetCategoriesForSearch)
            .WithName("GetCategoriesForSearch")
            .WithSummary("List catalog categories for search")
            .WithDescription("Get a list of category for search in the catalog.")
            .WithTags("Shop/Categories");

        return app;
    }

    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
    public static async Task<Results<Ok<PaginatedList<ItemCardDto>>, NotFound>> GetItemsByTitle(
        [Description("The catalog item title")] string title,
        [AsParameters] PaginationRequest paginationRequest,
        [AsParameters] CatalogServices services,
        CancellationToken cancellationToken)
    {
        var root = (IQueryable<CatalogItem>)services.Context.CatalogItems;

        if (title is not null)
        {
            root = root.Where(i => EF.Functions.ILike(i.Title, $"%{title}%"));
        }

        var items = await root
            .OrderBy(c => c.Title)
            .ProjectTo<ItemCardDto>(services.Mapper.ConfigurationProvider)
            .PaginatedListAsync(paginationRequest.PageNumber, paginationRequest.PageSize, cancellationToken);

        return TypedResults.Ok(items);
    }

    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
    public static async Task<Ok<ResultList<CategoryOutputDto>>> GetCategoriesForSearch(
        [AsParameters] CatalogServices services,
        CancellationToken cancellationToken)
    {
        var root = (IQueryable<CatalogCategory>)services.Context.CatalogCategories;

        var categories = await root
            .OrderBy(c => c.Index)
            .ProjectTo<CategoryOutputDto>(services.Mapper.ConfigurationProvider)
            .ToResultListAsync(cancellationToken);

        return TypedResults.Ok(categories);
    }


    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
    public static async Task<Results<Ok<PaginatedList<ItemCardDto>>, NotFound>> GetItemsByCategorySlug(
        [Description("The catalog item category-slug")] string categorySlug,
        [AsParameters] PaginationRequest paginationRequest,
        [AsParameters] CatalogServices services,
        CancellationToken cancellationToken)
    {
        var root = (IQueryable<CatalogItem>)services.Context.CatalogItems;

        if (!categorySlug.Equals("shop"))
        {
            if (categorySlug.Split(',').Length > 1)
            {
                var categorySlugs = categorySlug.Split(',');

                foreach (var item in categorySlugs)
                {
                    root = root.Where(c => c.CatalogCategories.Any(cate => EF.Functions.ILike(cate.Slug, $"%{item}")));
                }
            }
            else
            {
                root = root.Where(c => c.CatalogCategories.Any(cate => EF.Functions.ILike(cate.Slug, $"%{categorySlug}")));
            }
        }

        var items = await root
            .OrderBy(c => c.Title)
            .ProjectTo<ItemCardDto>(services.Mapper.ConfigurationProvider)
            .PaginatedListAsync(paginationRequest.PageNumber, paginationRequest.PageSize, cancellationToken);

        return TypedResults.Ok(items);
    }

    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
    public static async Task<Results<Ok<Result<ItemOutputDto>>, NotFound>> GetItemBySlug(
        [Description("The catalog item slug")] string slug,
        [AsParameters] CatalogServices services,
        CancellationToken cancellationToken)
    {
        if (slug is null)
        {
            return TypedResults.NotFound();
        }

        var root = (IQueryable<CatalogItem>)services.Context.CatalogItems;

        var item = await root
            .Where(ci => ci.Slug == slug)
            .ProjectTo<ItemOutputDto>(services.Mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        return item is null
            ? TypedResults.NotFound()
            : TypedResults.Ok(new Result<ItemOutputDto>(item));
    }

    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
    public static async Task<Ok<ResultList<ItemCardDto>>> GetPremiumItems(
        [AsParameters] CatalogServices services,
        CancellationToken cancellationToken)
    {
        var root = (IQueryable<CatalogItem>)services.Context.CatalogItems;

        var items = await root
            .Where(c => c.CatalogCategories.Any(cate => EF.Functions.ILike(cate.Slug, "%premium%")))
            .OrderBy(c => c.Slug)
            .Take(10)
            .ProjectTo<ItemCardDto>(services.Mapper.ConfigurationProvider)
            .ToResultListAsync(cancellationToken);

        return TypedResults.Ok(items);
    }

    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
    public static async Task<Ok<ResultList<CategoryOutputDto>>> GetCategoriesForNav(
        [AsParameters] CatalogServices services,
        CancellationToken cancellationToken)
    {
        var root = (IQueryable<CatalogCategory>)services.Context.CatalogCategories;

        var categories = await root
            .OrderBy(c => c.Index)
            .Take(5)
            .ProjectTo<CategoryOutputDto>(services.Mapper.ConfigurationProvider)
            .ToResultListAsync(cancellationToken);

        return TypedResults.Ok(categories);
    }
}
