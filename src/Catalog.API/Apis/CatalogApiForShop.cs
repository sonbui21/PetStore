namespace Catalog.API.Apis;

public static partial class CatalogApi
{
    public static IEndpointRouteBuilder MapCatalogApiForShop(this IEndpointRouteBuilder app)
    {
        // RouteGroupBuilder for catalog endpoints
        var api = app.MapGroup("api/shop/catalog");

        api.MapGet("/premium-items", GetPremiumItems);

        api.MapGet("/items/{slug:minlength(1)}", GetItemBySlug);

        api.MapGet("/items/category/{categorySlug:minlength(1)}", GetItemsByCategorySlug);

        api.MapGet("/items/title/{title:minlength(1)}", GetItemsByTitle);

        api.MapGet("/nav-categories", GetCategoriesForNav);

        api.MapGet("/search-categories", GetCategoriesForSearch);

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
    public static async Task<Ok<ResultList<CategoryForSearchDto>>> GetCategoriesForSearch(
        [AsParameters] CatalogServices services,
        CancellationToken cancellationToken)
    {
        var root = (IQueryable<Category>)services.Context.Categories;

        var categories = await root
            .OrderBy(c => c.Index)
            .ProjectTo<CategoryForSearchDto>(services.Mapper.ConfigurationProvider)
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
                    root = root.Where(c => c.Categories.Any(cate => EF.Functions.ILike(cate.Slug, $"%{item}")));
                }
            }
            else
            {
                root = root.Where(c => c.Categories.Any(cate => EF.Functions.ILike(cate.Slug, $"%{categorySlug}")));
            }
        }

        var items = await root
            .OrderBy(c => c.Title)
            .ProjectTo<ItemCardDto>(services.Mapper.ConfigurationProvider)
            .PaginatedListAsync(paginationRequest.PageNumber, paginationRequest.PageSize, cancellationToken);

        return TypedResults.Ok(items);
    }

    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
    public static async Task<Results<Ok<Result<CatalogItemDto>>, NotFound>> GetItemBySlug(
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
            .ProjectTo<CatalogItemDto>(services.Mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        return item is null
            ? TypedResults.NotFound()
            : TypedResults.Ok(new Result<CatalogItemDto>(item));
    }

    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
    public static async Task<Ok<ResultList<ItemCardDto>>> GetPremiumItems(
        [AsParameters] CatalogServices services,
        CancellationToken cancellationToken)
    {
        var root = (IQueryable<CatalogItem>)services.Context.CatalogItems;

        var items = await root
            .Where(c => c.Categories.Any(cate => EF.Functions.ILike(cate.Slug, "%premium%")))
            .OrderBy(c => c.Slug)
            .Take(10)
            .ProjectTo<ItemCardDto>(services.Mapper.ConfigurationProvider)
            .ToResultListAsync(cancellationToken);

        return TypedResults.Ok(items);
    }

    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
    public static async Task<Ok<ResultList<CategoryDto>>> GetCategoriesForNav(
        [AsParameters] CatalogServices services,
        CancellationToken cancellationToken)
    {
        var root = (IQueryable<Category>)services.Context.Categories;

        var categories = await root
            .OrderBy(c => c.Index)
            .Take(4)
            .ProjectTo<CategoryDto>(services.Mapper.ConfigurationProvider)
            .ToResultListAsync(cancellationToken);

        return TypedResults.Ok(categories);
    }
}
