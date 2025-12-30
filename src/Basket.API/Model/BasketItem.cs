namespace Basket.API.Model;

public class BasketItem : IValidatableObject
{
    public string Id { get; set; }
    public string ProductId { get; set; }
    public string VariantId { get; set; }
    public int Quantity { get; set; }

    public string Title { get; set; }
    public string Slug { get; set; }
    public string Thumbnail { get; set; }

    public List<VariantOption> VariantOptions { get; set; }
    public string Price { get; set; }
    public int AvailableStock { get; set; }


    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var results = new List<ValidationResult>();

        if (Quantity < 1)
        {
            results.Add(new ValidationResult("Invalid number of units", ["Quantity"]));
        }

        return results;
    }
}

public class VariantOption
{
    public string Name { get; set; }
    public string Value { get; set; }
}