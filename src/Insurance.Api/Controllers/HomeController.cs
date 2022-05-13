using System.Collections.Generic;
using System.Linq;
using Insurance.Api.Domain;
using Insurance.Api.Models;
using Insurance.Api.Domain.Exceptions;
using Insurance.Api.Gateways;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Insurance.Api.Controllers
{
    public class HomeController: Controller
    {

        private readonly ILogger _logger;
        private readonly IProductGateway _productGateway;
        private readonly IProductTypeGateway _productTypeGateway;
        public HomeController(IProductGateway productGateway,
            IProductTypeGateway productTypeGateway,
            ILogger<HomeController> logger)
        {
            _logger = logger;
            _productGateway = productGateway;
            _productTypeGateway = productTypeGateway;
        }

        [HttpPost]
        [Route("api/insurance/product")]
        public async Task<ActionResult<InsuranceResponseDto>> CalculateInsurance([FromBody] InsuranceRequestDto toInsure)
        {
            _logger.LogInformation("CalculateInsurance - Request initiated - {@toInsure}", toInsure);
            try
            {
                if (!ModelState.IsValid) {
                    string modelStateErrors = string.Join(" || ", ModelState.Values.SelectMany(s => s.Errors).Select(e => e.ErrorMessage));
                    _logger.LogWarning("CalculateInsurance - Validation Errors : {@modelStateErrors}", modelStateErrors);
                    return BadRequest(ModelState);
                }

                int productId = toInsure.ProductId.Value;

                var productDetail = await _productGateway.GetProduct(productId);
                if (productDetail == null)
                {
                    _logger.LogWarning("CalculateInsurance - Product not found : {@productId}", productId);
                    return NotFound();
                }

                var productType = await _productTypeGateway.GetProductType(productDetail.Value.ProductTypeId.Value);
                
                var product = ProductFactory.GetProduct(productDetail, productType);

                (int insuranceProductId, float insuranceValue) = product.InsuranceCost();

                return new InsuranceResponseDto() {
                    ProductId = insuranceProductId,
                    InsuranceValue = insuranceValue
                };
            }
            catch (ProductNotFoundException)
            {
                _logger.LogError("CalculateInsurance - Product not found : {@toInsure}", toInsure);
                return NotFound();
            }
        }

        [HttpPost]
        [Route("api/insurance/order")]
        public async Task<ActionResult<OrderResultDto>> CalculateOrderInsurance([FromBody] OrderDto orderInput)
        {
            _logger.LogInformation("CalculateOrderInsurance - Request initiated - {@orderInput}", orderInput);
            var prodIds = orderInput.Items.Select(x => x.ProductId);
            var products = new List<ProductDto?>();
            var productTypes = new List<ProductTypeSurchargeDto>();
            foreach (var id in prodIds)
            {
                var prod = await _productGateway.GetProduct(id);
                if (prod != null && prod.HasValue)
                {
                    if (!productTypes.Any(x => x.Id == prod.Value.ProductTypeId))
                    {
                        productTypes.Add(await _productTypeGateway.GetProductType(prod.Value.ProductTypeId.Value));
                    }
                    products.Add(prod);
                }
            }

            var order = OrderFactory.GetOrder(orderInput, products, productTypes);
            return new OrderResultDto { InsuranceValue = order.InsuranceCost() };
        }
    }
}