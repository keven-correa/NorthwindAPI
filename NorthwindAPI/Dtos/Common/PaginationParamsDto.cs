namespace NorthwindAPI.Dtos.Common
{
    public class PaginationParamsDto
    {
        public int Total { get; set; }
        public int CurrentPage { get; set; } = 1;
        private const int MaxPageSize = 50;

        private int _pageSize = 3;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
        }
        public int TotalPages { get; set; }
    }
}
