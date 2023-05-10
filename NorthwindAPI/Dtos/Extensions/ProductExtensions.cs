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

        //public static void UpdateFromDto(this Product product, ProductRequestDto productRequestDto, int id)
        //{
        //    product.ProductId = id;
        //    product.ProductName = productRequestDto.ProductName!;
        //    product.CategoryId = productRequestDto.CategoryId!;
        //    product.SupplierId = productRequestDto.SupplierId!;
        //    product.QuantityPerUnit = productRequestDto.QuantityPerUnit!;
        //    product.UnitPrice = productRequestDto.UnitPrice!;
        //    product.UnitsInStock = productRequestDto.UnitsInStock!;
        //    product.UnitsOnOrder = productRequestDto.UnitsOnOrder;
        //    product.ReorderLevel = productRequestDto.ReorderLevel;

        //}
        public static Product ToUpdateEntity(this ProductRequestUpdateDto productDto)
        {
            return new Product
            {
                ProductId= productDto.ProductId,
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
