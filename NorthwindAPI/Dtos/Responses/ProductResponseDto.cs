﻿namespace NorthwindAPI.Dtos.Responses
{
    public class ProductResponseDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int? CategoryId { get; set; }

        public string? QuantityPerUnit { get; set; }

        public decimal? UnitPrice { get; set; }

        public short? UnitsInStock { get; set; }

        public bool Discontinued { get; set; }

    }
}
