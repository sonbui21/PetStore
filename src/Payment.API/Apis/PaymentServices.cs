namespace Payment.API.Apis;

public class PaymentServices(
    IMediator mediator,
    IPaymentRepository paymentRepository,
    ILogger<PaymentServices> logger)
{
    public IMediator Mediator { get; set; } = mediator;
    public IPaymentRepository PaymentRepository { get; set; } = paymentRepository;

    public ILogger<PaymentServices> Logger { get; } = logger;
}
