using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NorthwindAPI.Dtos.Common;
using NorthwindAPI.Dtos.Responses;
using NorthwindAPI.Entities;

namespace NorthwindAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuppliersController : ControllerBase
    {
        private readonly NorthwindContext _context;

        public SuppliersController(NorthwindContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Supplier>>> GetAllSuppliers([FromQuery] PaginationParamsDto paginationParams)
        {

            if (_context.Products == null) return NotFound();

            var suppliersCount = await _context.Suppliers.CountAsync();

            var totalPages = (int)Math.Ceiling((double)suppliersCount / paginationParams.PageSize);
            var suppliers = await _context.Suppliers
                 .Select(s => new SuppliersResponseDto
                 {
                     SupplierId = s.SupplierId,
                     CompanyName = s.CompanyName,
                     ContactName = s.ContactName,
                     ContactTitle = s.ContactTitle,
                     Address = s.Address,
                     City = s.City,
                     Country = s.Country,
                     Phone = s.Phone,
                     Region = s.Region,
                     PostalCode = s.PostalCode,
                     Fax = s.Fax,
                     HomePage = s.HomePage
                 })
                 .Skip((paginationParams.CurrentPage - 1) * paginationParams.PageSize)
                 .Take(paginationParams.PageSize)
                 .AsNoTracking()
                 .ToListAsync();
            var response = new PaginationDto<SuppliersResponseDto>
            {
                Data = suppliers,
                Total = suppliersCount,
                CurrentPage = paginationParams.CurrentPage,
                PageSize = paginationParams.PageSize,
                PageCount = totalPages,
            };
            return Ok(response);
        }
        [HttpGet("supplier-with-products/{id}")]
        public async Task<ActionResult<SuppliersWithProductsDto>> SuppliersWithPorudcts(int id)
        {
            var supplier = await _context.Suppliers.Include(s => (s as Product).Products.Select(p => new ProductResponseDto {ProductId = p.ProductId, ProductName = p.ProductName, CategoryId = p.CategoryId, 
            Discontinued = p.Discontinued, UnitPrice = p.UnitPrice, QuantityPerUnit = p.QuantityPerUnit, SupplierId = p.SupplierId, UnitsInStock = p.UnitsInStock}))
                .FirstOrDefaultAsync(s => s.SupplierId == id);

            if (supplier == null) return NotFound();

            //var response =  _context.Suppliers.Select(s => new SuppliersWithProductsDto
            //{
            //    SupplierId = s.SupplierId,
            //    CompanyName= s.CompanyName,
            //    ContactName = s.ContactName,
            //    ContactTitle = s.ContactTitle,
            //    Address= s.Address,
            //    Country= s.Country,
            //    City    = s.City,
            //    Region = s.Region,
            //    PostalCode = s.PostalCode,
            //    Fax= s.Fax,
            //    HomePage= s.HomePage,
            //    Phone= s.Phone,
            //    Products = supplier.Products.Select(p => new ProductResponseDto
            //    {
            //        ProductId = p.ProductId,
            //        ProductName = p.ProductName,
            //        CategoryId= p.CategoryId,
            //        Discontinued    = p.Discontinued,
            //        QuantityPerUnit= p.QuantityPerUnit,
            //        SupplierId= p.SupplierId,
            //        UnitPrice= p.UnitPrice,
            //        UnitsInStock = p.UnitsInStock,
            //    })

            //});
            return Ok(supplier);   
        }
    }
}

