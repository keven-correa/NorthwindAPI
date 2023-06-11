using AutoMapper;
using Azure;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NorthwindAPI.Dtos;
using NorthwindAPI.Dtos.Common;
using NorthwindAPI.Dtos.Responses;
using NorthwindAPI.Entities;

namespace NorthwindAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly NorthwindContext _context;
        private readonly IMapper _mapper;

        public ProductsController(NorthwindContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<CreateProductDto>> PostProduct([FromBody] CreateProductDto productDto)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'NorthwindContext.Products'  is null.");
            }
            var product = _mapper.Map<Product>(productDto);
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProduct), new { id = product.ProductId }, product);
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
                SupplierId = x.SupplierId,
            })
                .Skip((paginationParams.CurrentPage - 1) * paginationParams.PageSize)
                .Take(paginationParams.PageSize)
                .AsNoTracking()
                .ToListAsync();

            var response = new
            {
                Results = products,
                Total = productsCount,
                CurrentPage = paginationParams.CurrentPage,
                PageSize = paginationParams.PageSize,
                PageCount = totalPages,
            };
            return Ok(response);

        }

        [HttpGet("search-by-name")]
        public async Task<ActionResult<ProductResponseDto>> SearchByName(string name)
        {
            var product = await _context.Products
                .Where(p => p.ProductName.Contains(name))
                .Select(p => new ProductResponseDto
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

            var products = await _context.Products
                .Select(x => new ProductResponseDto
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
            return await _context.ProductsByCategories
                .OrderBy(p => p.CategoryName)
                .ToArrayAsync();
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
                .OrderByDescending(p => p.UnitsInStock)
                .FirstOrDefaultAsync();

            return Ok(product);
        }

        [HttpGet("GetProduct/{id}")]
        public async Task<ActionResult<ProductResponseDto>> GetProduct(int id)
        {
            if (_context.Products == null)
            {
                return NotFound();
            }
            var product = await _context.Products.Select(x => new ProductResponseDto
            {
                ProductId = x.ProductId,
                ProductName = x.ProductName,
                QuantityPerUnit = x.QuantityPerUnit,
                UnitPrice = x.UnitPrice,
                UnitsInStock = x.UnitsInStock,
                CategoryId = x.CategoryId,
                Discontinued = x.Discontinued,
                SupplierId = x.SupplierId,
            }).FirstOrDefaultAsync(x => x.ProductId == id);

            return Ok(product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, CreateProductDto productDto)
        {
            if (!ProductExists(id))
            {
                return NotFound();
            }

            var product = _mapper.Map<Product>(productDto);
            product.ProductId = id;
            _context.Update(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<CreateProductDto>> PatchProduct(int id, JsonPatchDocument<CreateProductDto> patchDocument)
        {
            if (patchDocument == null)
                return BadRequest();

            var productExists = await _context.Products.FirstOrDefaultAsync(x => x.ProductId == id);

            var productDto = _mapper.Map<CreateProductDto>(productExists);
            patchDocument.ApplyTo(productDto, ModelState);
            var isValid = TryValidateModel(productDto);

            if (!isValid)
                return BadRequest(ModelState);

            _mapper.Map(productDto, productExists);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if (_context.Products == null)
            {
                return NotFound();
            }
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(int id)
        {
            return (_context.Products?.Any(e => e.ProductId == id)).GetValueOrDefault();
        }
    }
}
