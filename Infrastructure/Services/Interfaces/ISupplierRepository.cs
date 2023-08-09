using NorthwindAPI.Dtos.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.Interfaces
{
    public interface ISupplierRepository
    {
        Task<IEnumerable<SuppliersResponseDto>> GetAlSuppliersAsync();
    }
}
