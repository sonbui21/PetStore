namespace Catalog.API.Model;

public class Category
{
    public Guid Id { get; set; }
    public int Index { get; set; }
    public string Name { get; set; }
    public string Slug { get; set; }
    public string Description { get; set; }

    public ICollection<CatalogItem> CatalogItems { get; set; }
}

public class ItemOption
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public List<string> Values { get; set; }

    public Guid CatalogItemId { get; set; }
    public CatalogItem CatalogItem { get; set; }
}

public class ItemVariant
{
    public Guid Id { get; set; }
    public Guid CatalogItemId { get; set; }
    public CatalogItem CatalogItem { get; set; }

    public string Title { get; set; }
    public decimal Price { get; set; }
    public int AvailableStock { get; set; }

    public ICollection<ItemVariantOption> Options { get; set; }
}

public class ItemVariantOption
{
    public Guid Id { get; set; }
    public Guid ItemVariantId { get; set; }
    public ItemVariant ItemVariant { get; set; }

    public string Name { get; set; }
    public string Value { get; set; }
}