namespace Payment.API.Domain.SeedWork;

public interface IRepository<T>
{
    IUnitOfWork UnitOfWork { get; }
}
