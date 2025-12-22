namespace Identity.API.Services;

public class EFLoginService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager) : ILoginService<ApplicationUser>
{
    public async Task<ApplicationUser> FindByUsername(string user)
    {
        return await userManager.FindByEmailAsync(user);
    }

    public async Task<bool> ValidateCredentials(ApplicationUser user, string password)
    {
        return await userManager.CheckPasswordAsync(user, password);
    }

    public Task SignIn(ApplicationUser user)
    {
        return signInManager.SignInAsync(user, true);
    }

    public Task SignInAsync(ApplicationUser user, AuthenticationProperties properties, string authenticationMethod = null)
    {
        return signInManager.SignInAsync(user, properties, authenticationMethod);
    }
}
