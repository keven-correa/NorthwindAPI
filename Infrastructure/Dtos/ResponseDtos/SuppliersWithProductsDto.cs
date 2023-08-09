namespace NorthwindAPI.Dtos.Responses
{
    public class SuppliersWithProductsDto : SuppliersResponseDto
    {
        public List<ProductResponseDto> Products { get; set; }
    }
}
