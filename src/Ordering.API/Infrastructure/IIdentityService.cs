namespace Ordering.API.Infrastructure;

public interface IIdentityService
{
    string GetUserIdentity();

    string GetUserName();
}
