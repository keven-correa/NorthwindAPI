using NorthwindAPI.Entities;
using System.ComponentModel.DataAnnotations;

namespace NorthwindAPI.Dtos
{
    public class CreateProductDto
    {
        [Required(ErrorMessage = "Product name is mandatory")]
        [MaxLength(100)]
        public string? ProductName { get; set; }

        [Required(ErrorMessage = "The supplier of the product is mandatory")]
        public int? SupplierId { get; set; }

        [Required(ErrorMessage = "The category of the product is mandatory")]
        public int? CategoryId { get; set; }

        [Required(ErrorMessage = "The quantity per unit of the product is mandatory")]
        public string? QuantityPerUnit { get; set; }

        //[Range(0, 9999999999999999.99, ErrorMessage = "Invalid Target Price; Max 18 digits")]
        [Required(ErrorMessage ="The product price is mandatory")]
        public decimal? UnitPrice { get; set; }

        [Required(ErrorMessage = "Specify the quantity of products ")]
        public short? UnitsInStock { get; set; }

        public bool Discontinued { get; set; } = false;

        public short? UnitsOnOrder { get; set; } = 0;

        public short? ReorderLevel { get; set; } = 0;
    }
}
