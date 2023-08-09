using NorthwindAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthWindClient.Shared.Repository
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetProducts();
    }
}
