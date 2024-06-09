using Microsoft.EntityFrameworkCore;

namespace Interns.Common.Pagination
{
    public static class PaginationExtensions
    {
        public static Task<PaginatedItems<TEntity>> Paginated<TEntity>(
            this IQueryable<TEntity> query,
            int page,
            int maxPageSize
        ) => Paginated(query, page, maxPageSize, e => e);

        public static async Task<PaginatedItems<TResult>> Paginated<TEntity, TResult>(
            this IQueryable<TEntity> source,
            int page,
            int maxPageSize,
            Func<TEntity, TResult> mapper
        )
        {
            int totalItems = await source.CountAsync();
            var itemsPage = await source
                .Skip((page - 1) * maxPageSize)
                .Take(maxPageSize)
                .ToListAsync();

            return new PaginatedItems<TResult>()
            {
                PaginationInfo = new()
                {
                    CurrentPage = page,
                    TotalPages = totalItems,
                    PageSize = itemsPage.Count
                },
                Items = itemsPage.Select(mapper)
            };
        }
    }
}
