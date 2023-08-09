namespace NorthwindAPI.Dtos.Common
{
    public class PaginationDto2<T> where T : class
    {
        public List<T>? Data { get; set; }
        public int Total { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }

    }
}
