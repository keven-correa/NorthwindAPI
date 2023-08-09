namespace NorthwindAPI
{
    public class PaginationDto
    {
        public int Page { get; set; } = 1;
        private int quantityPerPage = 10;

        private readonly int MaxPerPage = 50;

        public int QuantityPerPage
        {
            get => quantityPerPage;
            set
            {
                quantityPerPage = (value > MaxPerPage) ? quantityPerPage : value;
            }   
        }


    }
}
