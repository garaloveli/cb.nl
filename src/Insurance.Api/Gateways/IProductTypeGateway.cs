using Insurance.Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Insurance.Api.Gateways
{
    public interface IProductTypeGateway
    {
        Task<ProductTypeSurchargeDto?> GetProductType(int productTypeId);
        //IEnumerable<ProductTypeSurchargeDto> GetProductTypes();
        Task SaveSurchargeRate(int productTypeId, float surchargeRate);
        float? GetSurchargeRate(int productTypeId);
    }
}
