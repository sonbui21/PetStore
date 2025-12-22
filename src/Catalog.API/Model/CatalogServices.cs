namespace Catalog.API.Model;

public class CatalogServices(
    CatalogContext context,
    IOptions<CatalogOptions> options,
    ILogger<CatalogServices> logger,
    [FromServices] ICatalogIntegrationEventService eventService,
    IMapper mapper)
{
    public CatalogContext Context { get; } = context;
    public IOptions<CatalogOptions> Options { get; } = options;
    public ILogger<CatalogServices> Logger { get; } = logger;
    public ICatalogIntegrationEventService EventService { get; } = eventService;
    public IMapper Mapper { get; } = mapper;
};
