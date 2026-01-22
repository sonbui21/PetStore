using Catalog.API.Infrastructure.Exceptions;

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

    public int RemoveStock(int quantityDesired)
    {
        if (AvailableStock == 0)
        {
            throw new CatalogDomainException($"Empty stock, product item {Title} is sold out");
        }

        if (quantityDesired <= 0)
        {
            throw new CatalogDomainException($"Item units desired should be greater than zero");
        }

        int removed = Math.Min(quantityDesired, this.AvailableStock);

        this.AvailableStock -= removed;

        return removed;
    }
}

public class ItemVariantOption
{
    public Guid Id { get; set; }
    public Guid ItemVariantId { get; set; }
    public ItemVariant ItemVariant { get; set; }

    public string Name { get; set; }
    public string Value { get; set; }
}