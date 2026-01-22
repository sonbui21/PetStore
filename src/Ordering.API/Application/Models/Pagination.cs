using System.ComponentModel;

namespace Ordering.API.Application.Models;

public record PaginationRequest(
    [property: Description("Number of items to return in a single page of results")]
    [property: DefaultValue(10)]
    int PageSize = 10,

    [property: Description("The number of the page of results to return")]
    [property: DefaultValue(1)]
    int PageNumber = 1
);

public record Pagination(
    int PageNumber,
    int TotalPages,
    int TotalCount,
    bool HasPreviousPage,
    bool HasNextPage
);

public class PaginatedList<T>(IReadOnlyCollection<T> data, int count, int pageNumber, int pageSize)
{
    public IReadOnlyCollection<T> Data { get; } = data;
    public Pagination Pagination { get; } = new Pagination(
        pageNumber,
        (int)Math.Ceiling(count / (double)pageSize),
        count,
        pageNumber > 1,
        pageNumber < (int)Math.Ceiling(count / (double)pageSize));

    public static async Task<PaginatedList<T>> CreateAsync(
        IQueryable<T> source,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var count = await source.CountAsync(cancellationToken);
        var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);

        return new PaginatedList<T>(items, count, pageNumber, pageSize);
    }
}
