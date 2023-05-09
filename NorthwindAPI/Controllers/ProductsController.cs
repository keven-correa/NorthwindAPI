using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NorthwindAPI.Dtos.Common;
using NorthwindAPI.Dtos.Management;
using NorthwindAPI.Dtos.Responses;
using NorthwindAPI.Entities;
using NuGet.Versioning;

namespace NorthwindAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly NorthwindContext _context;

        public ProductsController(NorthwindContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct([FromBody] CreateProductDto productDto)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'NorthwindContext.Products'  is null.");
            }
            var product = new Product()
            {
                ProductName = productDto.ProductName,
                CategoryId = productDto.CategoryId,
                SupplierId = productDto.SupplierId,
                QuantityPerUnit = productDto.QuantityPerUnit,
                UnitPrice = productDto.UnitPrice,
                UnitsInStock = productDto.UnitsInStock,
                Discontinued = productDto.Discontinued,
                UnitsOnOrder = productDto.UnitsOnOrder,
                ReorderLevel= productDto.ReorderLevel,
            };
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = product.ProductId }, product);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaginationDto<ProductResponseDto>>>> GetProducts([FromQuery] PaginationParamsDto paginationParams)
        {
            if (_context.Products == null) return NotFound();

            var productsCount = await _context.Products.CountAsync();

            var totalPages = (int)Math.Ceiling((double)productsCount / paginationParams.PageSize);

            var products = await _context.Products.Select(x => new ProductResponseDto
            {
                ProductId = x.ProductId,
                ProductName = x.ProductName,
                QuantityPerUnit = x.QuantityPerUnit,
                UnitPrice = x.UnitPrice,
                UnitsInStock = x.UnitsInStock,
                CategoryId = x.CategoryId,
                Discontinued = x.Discontinued,
            })
                .Skip((paginationParams.CurrentPage - 1) * paginationParams.PageSize)
                .Take(paginationParams.PageSize)
                .AsNoTracking()
                .ToListAsync();

            var response = new PaginationDto<ProductResponseDto>
            {
                Data = products,
                Total = productsCount,
                CurrentPage = paginationParams.CurrentPage,
                PageSize = paginationParams.PageSize,
                PageCount = totalPages,
            };
            return Ok(response);

        }

        [HttpGet("search-by-name")]
        public async Task<ActionResult<ProductResponseDto>> searchByName(string name)
        {
            var product = await _context.Products.Where(p => p.ProductName.Contains(name)).Select(p => new ProductResponseDto
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                CategoryId = p.CategoryId,
                Discontinued = p.Discontinued,
                QuantityPerUnit = p.QuantityPerUnit,
                UnitPrice = p.UnitPrice,
                UnitsInStock = p.UnitsInStock,
            }).ToListAsync();

            return Ok(product);
        }
        [HttpGet("discontinued")]
        public async Task<ActionResult<IEnumerable<PaginationDto<ProductResponseDto>>>> GetAllDiscotinuedProducts([FromQuery] PaginationParamsDto paginationParams)
        {
            if (_context.Products == null) return NotFound();

            var productsCount = await _context.Products.Where(p => p.Discontinued == true).CountAsync();

            var totalPages = (int)Math.Ceiling((double)productsCount / paginationParams.PageSize);

            var products = await _context.Products.Select(x => new ProductResponseDto
            {
                ProductId = x.ProductId,
                ProductName = x.ProductName,
                QuantityPerUnit = x.QuantityPerUnit,
                UnitPrice = x.UnitPrice,
                UnitsInStock = x.UnitsInStock,
                CategoryId = x.CategoryId,
                Discontinued = x.Discontinued,
            })
                .Where(p => p.Discontinued == true)
                .Skip((paginationParams.CurrentPage - 1) * paginationParams.PageSize)
                .Take(paginationParams.PageSize)
                .AsNoTracking()
                .ToListAsync();

            var response = new PaginationDto<ProductResponseDto>
            {
                Data = products,
                Total = productsCount,
                CurrentPage = paginationParams.CurrentPage,
                PageSize = paginationParams.PageSize,
                PageCount = totalPages,
            };
            return Ok(response);

        }

        [HttpGet("api/[controller]/count")]
        public async Task<ActionResult> CountProducts()
        {
            return Ok(await _context.Products.CountAsync());
        }

        [HttpGet("api/[controller]/getByCategory")]
        public async Task<ActionResult<IEnumerable<ProductsByCategory>>> GetProductsByCategory()
        {
            if (_context.Products == null)
            {
                return NotFound();
            }
            return await _context.ProductsByCategories.OrderBy(p => p.CategoryName).ToArrayAsync();


        }

        [HttpGet("highest-quantity")]
        public async Task<ActionResult<ProductResponseDto>> GetProductWithHighestQuantity()
        {
            var product = await _context.Products.Select(p => new ProductResponseDto
            {
                ProductId = p.ProductId,
                CategoryId = p.CategoryId,
                ProductName = p.ProductName,
                QuantityPerUnit = p.QuantityPerUnit,
                Discontinued = p.Discontinued,
                UnitPrice = p.UnitPrice,
                UnitsInStock = p.UnitsInStock,
            })
                .OrderByDescending(p => p.UnitsInStock).FirstOrDefaultAsync();

            return Ok(product);
        }

        [HttpGet("GetProduct/{id}")]
        public async Task<ActionResult<ProductResponseDto>> GetProduct(int id)
        {
            if (_context.Products == null)
            {
                return NotFound();
            }
            var product = await _context.Products.Select(p => new ProductResponseDto
            {
                ProductId = p.ProductId,
                CategoryId = p.CategoryId,
                ProductName = p.ProductName,
                QuantityPerUnit = p.QuantityPerUnit,
                Discontinued = p.Discontinued,
                UnitPrice = p.UnitPrice,
                UnitsInStock = p.UnitsInStock,
            }).FirstOrDefaultAsync(x => x.ProductId == id);

            return Ok(product);
        }

        // PUT: api/Products/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutProduct(int id, Product product)
        //{
        //    if (id != product.ProductId)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(product).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!ProductExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}


        //// DELETE: api/Products/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteProduct(int id)
        //{
        //    if (_context.Products == null)
        //    {
        //        return NotFound();
        //    }
        //    var product = await _context.Products.FindAsync(id);
        //    if (product == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Products.Remove(product);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        private bool ProductExists(int id)
        {
            return (_context.Products?.Any(e => e.ProductId == id)).GetValueOrDefault();
        }
    }
}
