namespace Identity.API.Controllers.Home;

public class HomeController(
    IIdentityServerInteractionService interaction,
    IWebHostEnvironment environment,
    ILogger<HomeController> logger) : Controller
{
    public async Task<IActionResult> Index()
    {
        if (environment.IsDevelopment())
        {
            var result = await HttpContext.AuthenticateAsync();
            if (result.Principal is not null)
            {
                var model = new DiagnosticsViewModel(result);
                if (model is not null)
                {
                    return View(model);
                }
            }

            // only show in development
            return View();
        }

        logger.LogInformation("Homepage is disabled in production. Returning 404.");
        return NotFound();
    }

    /// <summary>
    /// Shows the error page
    /// </summary>
    public async Task<IActionResult> Error(string errorId)
    {
        var vm = new ErrorViewModel();

        // retrieve error details from identityserver
        var message = await interaction.GetErrorContextAsync(errorId);
        if (message != null)
        {
            vm.Error = message;

            if (!environment.IsDevelopment())
            {
                // only show in development
                message.ErrorDescription = null;
            }
        }

        return View("Error", vm);
    }
}
