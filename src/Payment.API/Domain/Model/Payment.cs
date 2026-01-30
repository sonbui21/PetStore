namespace Payment.API.Domain.Model;

public class Payment : Entity
{
    public string PaymentIntentId { get; set; }
    public Guid OrderId { get; set; }
    public long Amount { get; set; }
    public string Status { get; set; }
    public string ClientSecret { get; set; }
}
