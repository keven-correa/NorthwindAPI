using NorthwindAPI.Dtos.Management;
using System;
using System.Collections.Generic;

namespace NorthwindAPI.Entities;

public partial class Product
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = null!;

    public int? SupplierId { get; set; }

    public int? CategoryId { get; set; }

    public string? QuantityPerUnit { get; set; }

    public decimal? UnitPrice { get; set; }

    public short? UnitsInStock { get; set; }

    public short? UnitsOnOrder { get; set; }

    public short? ReorderLevel { get; set; }

    public bool Discontinued { get; set; }

    public virtual Category? Category { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; } = new List<OrderDetail>();

    public  Supplier? Supplier { get; set; }

    public static implicit operator Product(ProductRequestUpdateDto v)
    {
        return new ProductRequestUpdateDto
        {
            ProductName = v.ProductName,
            SupplierId = v.SupplierId,
            CategoryId = v.CategoryId,
            QuantityPerUnit = v.QuantityPerUnit,
            UnitPrice = v.UnitPrice,
            UnitsInStock = v.UnitsInStock,
            UnitsOnOrder = v.UnitsOnOrder,
            Discontinued = v.Discontinued,
            ReorderLevel = v.ReorderLevel,
        };
    }
}
