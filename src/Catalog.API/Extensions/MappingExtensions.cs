namespace Catalog.API.Extensions;

public static class MappingExtensions
{
    public static Task<PaginatedList<TDestination>> PaginatedListAsync<TDestination>(
        this IQueryable<TDestination> queryable,
        int pageNumber = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default) where TDestination : class
        => PaginatedList<TDestination>.CreateAsync(queryable.AsNoTracking(), pageNumber, pageSize, cancellationToken);

    public static Task<ResultList<TDestination>> ToResultListAsync<TDestination>(
        this IQueryable<TDestination> queryable,
        CancellationToken cancellationToken = default) where TDestination : class
        => ResultList<TDestination>.CreateAsync(queryable.AsNoTracking(), cancellationToken);

    public static Task<List<TDestination>> ProjectToListAsync<TDestination>(
        this IQueryable queryable,
        AutoMapper.IConfigurationProvider configuration,
        CancellationToken cancellationToken = default) where TDestination : class
        => queryable.ProjectTo<TDestination>(configuration).AsNoTracking().ToListAsync(cancellationToken);
}
