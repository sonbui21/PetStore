namespace Basket.API.Model;

public class BasketItemModel : IValidatableObject
{
    public string ProductId { get; set; }
    public string VariantId { get; set; }
    public int Quantity { get; set; }
    public string Title { get; set; }
    public string Slug { get; set; }
    public string Thumbnail { get; set; }
    public string Price { get; set; }
    public int AvailableStock { get; set; }
    public string VariantOptions { get; set; }


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
