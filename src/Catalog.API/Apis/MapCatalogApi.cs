namespace Catalog.API.Apis;

public static class CatalogApi
{
    public static IEndpointRouteBuilder MapCatalogApi(this IEndpointRouteBuilder app)
    {
        // RouteGroupBuilder for catalog endpoints
        var api = app.MapGroup("api/admin/catalog");

        api.MapGet("items", GetItems)
            .WithName("ListItems")
            .WithSummary("List catalog items")
            .WithDescription("Get a paginated list of items in the catalog.")
            .WithTags("Admin/Items");

        // Routes for querying category.
        api.MapGet("/categories", GetCategories)
            .WithName("ListCategories")
            .WithSummary("List catalog categories")
            .WithDescription("Get a paginated list of category in the catalog.")
            .WithTags("Admin/Categories");

        api.MapPost("/categories", CreateCategory)
            .WithName("CreateCategory")
            .WithSummary("Create a new category")
            .WithDescription("Create a new category in the catalog.")
            .WithTags("Admin/Categories");

        api.MapPut("/categories/{id}", UpdateCategoryById)
            .WithName("UpdateCategory")
            .WithSummary("Update a category")
            .WithDescription("Update an existing category in the catalog.")
            .WithTags("Admin/Categories");

        api.MapDelete("/categories/{id}", DeleteCategoryById)
            .WithName("DeleteCategory")
            .WithSummary("Delete a category")
            .WithDescription("Delete a category from the catalog.")
            .WithTags("Admin/Categories");

        return app;
    }

    
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
    public static async Task<Ok<PaginatedList<CatalogItemDto>>> GetItems(
        [Description("The title of the item to return")] string title,
        [Description("The slug of items to return")] string slug,
        [AsParameters] PaginationRequest paginationRequest,
        [AsParameters] CatalogServices services,
        CancellationToken cancellationToken)
    {
        var pageSize = paginationRequest.PageSize;
        var pageNumber = paginationRequest.PageNumber;

        var root = (IQueryable<CatalogItem>)services.Context.CatalogItems;

        if (title is not null)
        {
            root = root.Where(c => c.Title.StartsWith(title));
        }

        if (slug is not null)
        {
            root = root.Where(c => c.Slug.Equals(slug));
        }

        var items = await root
            .OrderBy(c => c.Title)
            .ProjectTo<CatalogItemDto>(services.Mapper.ConfigurationProvider)
            .PaginatedListAsync(pageNumber, pageSize, cancellationToken);

        return TypedResults.Ok(items);
    }

    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
    public static async Task<Ok<PaginatedList<CatalogCategoryDto>>> GetCategories(

        [Description("The id of the category to return")] string id,
        [Description("The name of the category to return")] string name,
        [Description("The slug of category to return")] string slug,
        [AsParameters] PaginationRequest paginationRequest,
        [AsParameters] CatalogServices services,
        CancellationToken cancellationToken)
    {
        var pageSize = paginationRequest.PageSize;
        var pageNumber = paginationRequest.PageNumber;

        var root = (IQueryable<Category>)services.Context.Categories;

        if (id is not null)
        {
            root = Guid.TryParse(id, out Guid result)
                ? root.Where(c => c.Id == result)
                : root.Where(c => false);
        }

        if (name is not null)
        {
            root = root.Where(c => EF.Functions.ILike(c.Name, $"%{name}%"));
        }

        if (slug is not null)
        {
            root = root.Where(c => c.Slug.Equals(slug));
        }

        var items = await root
            .OrderBy(c => c.Name)
            .ProjectTo<CatalogCategoryDto>(services.Mapper.ConfigurationProvider)
            .PaginatedListAsync(pageNumber, pageSize, cancellationToken);

        return TypedResults.Ok(items);
    }

    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
    public static async Task<Results<Created, BadRequest<ProblemDetails>>> CreateCategory(
        CreateCatalogCategoryRequest request,
        [AsParameters] CatalogServices services,
        CancellationToken cancellationToken)
    {
        var existingCategory = await services.Context.Categories.FirstOrDefaultAsync(c => c.Slug == request.Slug, cancellationToken);
        if (existingCategory != null)
        {
            return TypedResults.BadRequest<ProblemDetails>(new() { Detail = $"A category with slug '{request.Slug}' already exists." });
        }

        var category = services.Mapper.Map<Category>(request);
        category.Id = Guid.NewGuid();

        services.Context.Categories.Add(category);
        await services.Context.SaveChangesAsync(cancellationToken);

        return TypedResults.Created($"/api/catalog/categories/{category.Id}");
    }

    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
    public static async Task<Results<NoContent, NotFound, BadRequest<ProblemDetails>>> DeleteCategoryById(
        [Description("The id of the category item to delete")] Guid id,
        [AsParameters] CatalogServices services,
        CancellationToken cancellationToken)
    {
        var category = await services.Context.Categories.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        if (category is null)
        {
            return TypedResults.NotFound();
        }

        var hasItems = await services.Context.CatalogItems.AnyAsync(ci => ci.Categories.Any(cc => cc.Id == id), cancellationToken);
        if (hasItems)
        {
            return TypedResults.BadRequest<ProblemDetails>(new() { Detail = "Cannot delete category that has associated items." });
        }

        services.Context.Categories.Remove(category);
        await services.Context.SaveChangesAsync(cancellationToken);

        return TypedResults.NoContent();
    }

    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
    public static async Task<Results<Created, BadRequest<ProblemDetails>, NotFound<ProblemDetails>>> UpdateCategoryById(
        [Description("The id of the category item to update")] Guid id,
        UpdateCatalogCategoryRequest request,
        [AsParameters] CatalogServices services,
        CancellationToken cancellationToken)
    {
        var category = await services.Context.Categories.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        if (category is null)
        {
            return TypedResults.NotFound<ProblemDetails>(new() { Detail = $"Item with id {id} not found." });
        }

        if (category.Slug != request.Slug)
        {
            var existingCategory = await services.Context.Categories
                .FirstOrDefaultAsync(c => c.Slug == request.Slug && c.Id != id, cancellationToken);
            if (existingCategory != null)
            {
                return TypedResults.NotFound<ProblemDetails>(new() { Detail = $"A category with slug '{request.Slug}' already exists." });
            }
        }

        services.Mapper.Map(request, category);
        await services.Context.SaveChangesAsync(cancellationToken);

        return TypedResults.Created($"/api/catalog/categories/{id}");
    }
}
