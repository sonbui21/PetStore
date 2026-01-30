namespace Payment.API.Application.Commands;

[DataContract]
public class CreatePaymentCommand : IRequest<bool>
{
    [DataMember]
    public string PaymentIntentId { get; private set; }

    [DataMember]
    public Guid OrderId { get; private set; }

    [DataMember]
    public long Amount { get; private set; }

    [DataMember]
    public string Status { get; private set; }

    [DataMember]
    public string ClientSecret { get; private set; }

    public CreatePaymentCommand()
    {
    }

    public CreatePaymentCommand(string paymentIntentId, Guid orderId, long amount, string status, string clientSecret)
    {
        PaymentIntentId = paymentIntentId;
        OrderId = orderId;
        Amount = amount;
        Status = status;
        ClientSecret = clientSecret;
    }
}
