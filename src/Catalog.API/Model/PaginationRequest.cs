namespace Catalog.API.Model;

public record PaginationRequest(
    [property: Description("Number of items to return in a single page of results")]
    [property: DefaultValue(10)]
    int PageSize = 10,

    [property: Description("The number of the page of results to return")]
    [property: DefaultValue(1)]
    int PageNumber = 1
);
