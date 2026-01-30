namespace Payment.API.Domain.Exceptions;

/// Exception type for domain exceptions
/// </summary>
public class PaymentDomainException : Exception
{
    public PaymentDomainException()
    { }

    public PaymentDomainException(string message)
        : base(message)
    { }

    public PaymentDomainException(string message, Exception innerException)
        : base(message, innerException)
    { }
}
