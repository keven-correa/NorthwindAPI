using NorthwindAPI.Entities;

namespace NorthwindAPI.Dtos.Management
{
    public class CreateProductDto
    {
        public string? ProductName { get; set; }
        public int? SupplierId { get; set; }
        public int? CategoryId { get; set; }
        public string? QuantityPerUnit { get; set; }
        public decimal? UnitPrice { get; set; }
        public short? UnitsInStock { get; set; }
        public bool Discontinued { get; set; } = false;
        public short? UnitsOnOrder { get; set; } = 0;
        public short? ReorderLevel { get; set; } = 0;

        public static implicit operator CreateProductDto(Product product)
        {
            return new CreateProductDto
            {
                ProductName = product.ProductName,
                CategoryId = product.CategoryId,
                SupplierId = product.SupplierId,
                QuantityPerUnit = product.QuantityPerUnit,
                UnitPrice = product.UnitPrice,
                UnitsInStock = product.UnitsInStock,
                Discontinued = product.Discontinued,
                UnitsOnOrder = product.UnitsOnOrder,
                ReorderLevel = product.ReorderLevel,
            };
        }

    }
}
