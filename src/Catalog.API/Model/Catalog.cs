namespace Catalog.API.Model;

public class CatalogCategory
{
    public Guid Id { get; set; }
    public int Index { get; set; }
    public string Name { get; set; }
    public string Slug { get; set; }
    public string Description { get; set; }

    public ICollection<CatalogItem> CatalogItems { get; set; }
}

public class CatalogItemOption
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public List<string> Values { get; set; }
    public decimal? PriceAdjustment { get; set; }

    public Guid CatalogItemId { get; set; }
    public CatalogItem CatalogItem { get; set; }
}

public class CatalogItemVariant
{
    public Guid Id { get; set; }
    public Guid CatalogItemId { get; set; }
    public CatalogItem CatalogItem { get; set; }

    public string Title { get; set; }
    public decimal Price { get; set; }
    public string CurrencyCode { get; set; } = "USD";

    // Inventory per variant
    public int AvailableStock { get; set; }
    public bool AvailableForSale => AvailableStock > 0;

    // Selected options for this variant
    public ICollection<CatalogItemVariantOption> SelectedOptions { get; set; }
}

public class CatalogItemVariantOption
{
    public Guid Id { get; set; }

    public Guid CatalogItemVariantId { get; set; }
    public CatalogItemVariant CatalogItemVariant { get; set; }

    public string Name { get; set; }
    public string Value { get; set; }
}