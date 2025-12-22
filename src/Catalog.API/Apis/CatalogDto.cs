namespace Catalog.API.Apis;

public record CatalogItemDto
{
    public string Id { get; set; }
    public string Slug { get; set; }
    public string Title { get; set; }
    public string ShortDescription { get; set; }
    public string Description { get; set; }
    public int? Reviews { get; set; }
    public decimal? Rating { get; set; }
    public int? Sold { get; set; }
    public decimal Price { get; set; }
    public List<string> ImageUrl { get; set; }
}

public record CatalogCategoryDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Slug { get; set; }
    public string Description { get; set; }
}

public record CreateCatalogCategoryRequest
{
    public string Name { get; set; }
    public string Slug { get; set; }
    public string Description { get; set; }
}

public record UpdateCatalogCategoryRequest
{
    public string Name { get; set; }
    public string Slug { get; set; }
    public string Description { get; set; }
}

public record CategoryOutputDto
{
    public string Name { get; set; }
    public string Slug { get; set; }
    public int TotalCount { get; set; }
}

public record ItemCardDto
{
    public string Slug { get; set; }
    public string Title { get; set; }
    public string ImageUrl { get; set; }
    public decimal Price { get; set; }
    public decimal? OldPrice { get; set; }
}

public record ItemOutputDto
{
    public string Id { get; set; }
    public string Slug { get; set; }
    public string Title { get; set; }
    public decimal Price { get; set; }
    public decimal? OldPrice { get; set; }
    public string CurrencyCode { get; set; }
    public List<ImageDto> Images { get; set; }
    public List<ItemOptionDto> Options { get; set; }
    public List<CategoryOutputDto> Categories { get; set; }
    public List<VariantOutputDto> Variants { get; set; }

}

public record ItemOptionDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public List<string> Values { get; set; }
}

public record ImageDto
{
    public string Src { get; set; }
    public string Alt { get; set; }
}


public record VariantSelectedOptionDto
{
    public string Name { get; set; }
    public string Value { get; set; }
};

public record VariantOutputDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public int AvailableStock { get; set; }
    public bool AvailableForSale { get; set; }
    public decimal Price { get; set; }
    public string CurrencyCode { get; set; }
    public List<VariantSelectedOptionDto> SelectedOptions { get; set; }
}