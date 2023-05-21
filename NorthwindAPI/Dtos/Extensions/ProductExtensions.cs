using NorthwindAPI.Dtos.Management;
using NorthwindAPI.Entities;

namespace NorthwindAPI.Dtos.Extensions
{
    public static class ProductExtensions
    {
        public static ProductRequestDto ToDto(this Product product)
        {
            return new ProductRequestDto
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

        public static Product ToEntity(this ProductRequestDto productDto)
        {
            return new Product
            {
                ProductName = productDto.ProductName!,
                CategoryId = productDto.CategoryId,
                SupplierId = productDto.SupplierId,
                QuantityPerUnit = productDto.QuantityPerUnit,
                UnitPrice = productDto.UnitPrice,
                UnitsInStock = productDto.UnitsInStock,
                Discontinued = productDto.Discontinued,
                UnitsOnOrder = productDto.UnitsOnOrder,
                ReorderLevel = productDto.ReorderLevel,
            };
        }
    }
}
