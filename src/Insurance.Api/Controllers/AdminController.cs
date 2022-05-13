using Insurance.Api.Models;
using Insurance.Api.Gateways;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace Insurance.Api.Controllers
{
    [Route("api/admin")]
    public class AdminController : Controller
    {
        private readonly ILogger _logger;
        private readonly IProductTypeGateway _productTypeGateway;
        public AdminController(IProductTypeGateway productTypeGateway, ILogger<AdminController> logger)
        {
            _productTypeGateway = productTypeGateway;
            _logger = logger;
        }

        [HttpPost]
        [Route("surcharge/producttype")]
        public async Task<ActionResult> SurchargeProductType([FromBody] SurchargeRequestDto request)
        {
            _logger.LogInformation("SurchargeProductType - Request Initiated - {@request}", request);
            if(!ModelState.IsValid)
            {
                string modelStateErrors = string.Join(" || ", ModelState.Values.SelectMany(s => s.Errors).Select(e => e.ErrorMessage));
                _logger.LogWarning("SurchargeProductType - Validation Errors : {@modelStateErrors}", modelStateErrors);
                return BadRequest(ModelState);
            } 
            
            int id = request.ProductTypeId.Value;
            var productType = await _productTypeGateway.GetProductType(id);
            if (productType == null) {
                _logger.LogWarning("SurchargeProductType - Product not found : {@id}", id);
                return NotFound();
            }

            await _productTypeGateway.SaveSurchargeRate(id, request.SurchargeRate.Value);

            _logger.LogInformation("SurchargeProductType - Request ended");
            
            return Ok();
        }

        [HttpGet]
        [Route("surcharge/producttype/{id}")]
        public async Task<ActionResult<SurchargeResponseDto>> SurchargeProductType(int id)
        {
            _logger.LogInformation("SurchargeProductType - Request initiated - {@id}", id);

            var productType = await _productTypeGateway.GetProductType(id);
            if (productType == null)
            {
                _logger.LogWarning("SurchargeProductType - Product Type not found : {@id}", id);
                return NotFound();
            }

            var surcharge = _productTypeGateway.GetSurchargeRate(id);
            if (surcharge == null)
            {
                _logger.LogWarning("SurchargeProductType - Surcharge rate not found : {@id}", id);
                return NotFound();
            }

            _logger.LogInformation("SurchargeProductType - Request ended");

            return new SurchargeResponseDto
            {
                ProductTypeId = id,
                SurchargeRate = surcharge.Value
            };
        }
    }
}
