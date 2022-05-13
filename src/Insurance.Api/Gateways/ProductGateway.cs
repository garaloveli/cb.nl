using Insurance.Api.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Api.Gateways
{
    public class ProductGateway : IProductGateway
    {
        private ProductAPIOptions options;
        private ILogger _logger;
        public ProductGateway(IOptions<ProductAPIOptions> options,
            ILogger<ProductGateway> logger)
        {
            this._logger = logger;
            this.options = options.Value;
        }

        public async Task<ProductDto?> GetProduct(int productId)
        {
            HttpClient client = new HttpClient
            {
                BaseAddress = new Uri(options.Address)
            };
            var response = await client.GetAsync(string.Format("/products/{0:G}", productId));
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    var json = await response.Content.ReadAsStringAsync();
                    if (string.IsNullOrEmpty(json))
                    {
                        return null;
                    }
                    var product = JsonConvert.DeserializeObject<ProductDto>(json);
                    return product;
                case HttpStatusCode.NotFound:
                    return null;
                default:
                    throw new ApplicationException($" { response.StatusCode } ");
            }
        }

        public async Task<IEnumerable<ProductDto>> GetProducts()
        {
            HttpClient client = new HttpClient
            {
                BaseAddress = new Uri(options.Address)
            };
            var response = await client.GetAsync("/products");
            var json = await response.Content.ReadAsStringAsync();
            var products = JsonConvert.DeserializeObject<IEnumerable<ProductDto>>(json);
            return products;
        }
    }
}
