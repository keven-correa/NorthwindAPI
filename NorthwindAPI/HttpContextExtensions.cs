using Microsoft.EntityFrameworkCore;

namespace NorthwindAPI
{
    public static class HttpContextExtensions
    {
        public async static Task PaginationParams<T>(this HttpContext httpContext, IQueryable<T> queryable, int quantityPerPage)
        {
            double quantity = await queryable.CountAsync();
            double quantityPage = Math.Ceiling(quantity / quantityPerPage);
            httpContext.Response.Headers.Add("quantityPages", quantityPage.ToString());
        }
    }
}
