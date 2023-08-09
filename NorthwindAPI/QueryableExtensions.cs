namespace NorthwindAPI
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> Paginate<T>(this IQueryable<T> queryable, PaginationDto paginationDto)
        {
            return queryable
                .Skip((paginationDto.Page - 1) * paginationDto.QuantityPerPage)
                .Take(paginationDto.QuantityPerPage);
        }
    }
}
