using Insurance.Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Insurance.Api.Gateways
{
    public interface IProductGateway
    {
        Task<ProductDto?> GetProduct(int productId);
    }
}
