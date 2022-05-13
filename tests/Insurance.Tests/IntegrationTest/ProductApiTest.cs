using Insurance.Api.Gateways;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Insurance.Tests.IntegrationTest
{
    public class ProductApiTest
    {
        const string ADDRESS = "http://localhost:5002";
        static readonly IOptions<ProductAPIOptions> OPTIONS = Options.Create(
            new ProductAPIOptions() { Address = ADDRESS });
        static readonly ILogger<ProductGateway> LOGGER = NullLoggerFactory.Instance.CreateLogger<ProductGateway>();
        static readonly ILogger<ProductTypeGateway> PROD_TYPE_LOGGER = NullLoggerFactory.Instance.CreateLogger<ProductTypeGateway>();

        [Fact]
        public void ProductApi_GetProduct()
        {
            var gateway = new ProductGateway(OPTIONS, LOGGER);
            var products = gateway.GetProducts();
            Assert.NotNull(products);
        }

    }
}
