using Catalog.API.Infrastructure.Exceptions;

namespace Catalog.API.Model;

public class CatalogItem
{
    public Guid Id { get; set; }
    public string Slug { get; set; }
    public string Title { get; set; }
    public decimal Price { get; set; }
    public string CurrencyCode { get; set; }
    public List<string> Images { get; set; }

    public ICollection<Category> Categories { get; set; }
    public ICollection<ItemOption> ItemOptions { get; set; }
    public ICollection<ItemVariant> ItemVariants { get; set; }
}
