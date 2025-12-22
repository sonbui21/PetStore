namespace Catalog.API.Model;

public class ResultList<T>(IReadOnlyCollection<T> data, int count)
{
    public IReadOnlyCollection<T> Data { get; } = data;
    public int TotalCount { get; } = count;

    public static async Task<ResultList<T>> CreateAsync(
        IQueryable<T> source,
        CancellationToken cancellationToken = default)
    {
        var count = await source.CountAsync(cancellationToken);
        var items = await source.ToListAsync(cancellationToken);

        return new ResultList<T>(items, count);
    }
}

public class Result<T>(T data)
{
    public T Data { get; } = data;
}