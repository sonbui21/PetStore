namespace Identity.API.Controllers.Account;

public class LoginInputModel
{
    [Required]
    public string Username { get; set; }
    [Required]
    public string Password { get; set; }
    public bool RememberLogin { get; set; }
    public string ReturnUrl { get; set; }
}

public class LoginViewModel : LoginInputModel
{
    public bool AllowRememberLogin { get; set; } = true;
    public bool EnableLocalLogin { get; set; } = true;

    public IEnumerable<ExternalProvider> ExternalProviders { get; set; } = [];
    public IEnumerable<ExternalProvider> VisibleExternalProviders => ExternalProviders.Where(x => !string.IsNullOrWhiteSpace(x.DisplayName));

    public bool IsExternalLoginOnly => !EnableLocalLogin && ExternalProviders?.Count() == 1;
    public string ExternalLoginScheme => IsExternalLoginOnly ? ExternalProviders?.SingleOrDefault()?.AuthenticationScheme : null;
}

public class LogoutInputModel
{
    public string LogoutId { get; set; }
}

public class LogoutViewModel : LogoutInputModel
{
    public bool ShowLogoutPrompt { get; set; } = true;
}

public class LoggedOutViewModel
{
    public string PostLogoutRedirectUri { get; set; }
    public string ClientName { get; set; }
    public string SignOutIframeUrl { get; set; }

    public bool AutomaticRedirectAfterSignOut { get; set; }

    public string LogoutId { get; set; }
    public bool TriggerExternalSignout => ExternalAuthenticationScheme != null;
    public string ExternalAuthenticationScheme { get; set; }
}

public class RedirectViewModel
{
    public string RedirectUrl { get; set; }
}

public static class AccountOptions
{
    public static bool AllowLocalLogin { get; set; } = true;
    public static bool AllowRememberLogin { get; set; } = true;
    public static TimeSpan RememberMeLoginDuration { get; set; } = TimeSpan.FromDays(30);
    public static bool ShowLogoutPrompt { get; set; } = false;
    public static bool AutomaticRedirectAfterSignOut { get; set; } = true;
    public static string InvalidCredentialsErrorMessage { get; set; } = "Invalid username or password";
}