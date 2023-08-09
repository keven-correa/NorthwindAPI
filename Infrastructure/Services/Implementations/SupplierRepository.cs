using Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using NorthwindAPI.Dtos.Responses;
using NorthwindAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.Implementations
{
    public class SupplierRepository : ISupplierRepository
    {
        private readonly NorthwindContext _context;

        public SupplierRepository(NorthwindContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<SuppliersResponseDto>> GetAlSuppliersAsync()
        {
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
                .AsNoTracking()
                .ToListAsync();
            return suppliers;
        }
    }
}
