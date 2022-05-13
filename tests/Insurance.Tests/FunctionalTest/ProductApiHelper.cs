using Insurance.Api.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Tests.FunctionalTest
{
    internal class ProductApiHelper
    {
        private static readonly HttpClient _httpClient = new HttpClient()
        {
            BaseAddress = new Uri(TestConstants.PRODUCT_API_URL)
        };
        private static int[] SPECIAL_TYPES_ARRAY = { TestConstants.LAPTOP_TYPE_ID, TestConstants.SMARTPHONE_TYPE_ID, TestConstants.DIGITAL_CAMERA_TYPE_ID };

        public static int GetProductPerType(int productTypeId) {
            var response = _httpClient.GetAsync("/products").Result;

            response.EnsureSuccessStatusCode();

            var json = response.Content.ReadAsStringAsync().Result;

            var result = JsonConvert.DeserializeObject<IEnumerable<ProductDto>>(json);

            return result.Where(x => x.ProductTypeId == productTypeId).Select(x => x.Id.Value).First();
        }

        public static int GetProductPerTypeAndSalesPrice(int productTypeId, float minSalesPrice, float? maxSalesPrice = null)
        {
            var response = _httpClient.GetAsync("/products").Result;

            response.EnsureSuccessStatusCode();

            var json = response.Content.ReadAsStringAsync().Result;

            var result = JsonConvert.DeserializeObject<IEnumerable<ProductDto>>(json);

            var query = result.Where(x => x.ProductTypeId == productTypeId && x.SalesPrice >= minSalesPrice);
            if (maxSalesPrice.HasValue)
            {
                query = query.Where(x => x.SalesPrice <= maxSalesPrice.Value);
            }

            return query.Select(x => x.Id.Value).First();
        }


        public static int[] GetProductsPerTypeAndSalesPrice(int quantity, int? productTypeId = null, float? minSalesPrice = null, float? maxSalesPrice = null)
        {
            var response = _httpClient.GetAsync("/products").Result;

            response.EnsureSuccessStatusCode();

            var json = response.Content.ReadAsStringAsync().Result;

            var result = JsonConvert.DeserializeObject<IEnumerable<ProductDto>>(json);

            IEnumerable<ProductDto> query;

            if (productTypeId.HasValue)
            {
                query = result.Where(x => x.ProductTypeId == productTypeId);
            }
            else
            {
                query = result.Where(x => !SPECIAL_TYPES_ARRAY.Any(st => st == x.ProductTypeId));
            }

            if (maxSalesPrice.HasValue)
            {
                query = query.Where(x => x.SalesPrice <= maxSalesPrice.Value);
            }

            if (minSalesPrice.HasValue)
            {
                query = query.Where(x => x.SalesPrice >= minSalesPrice.Value);
            }

            return query.Select(x => x.Id.Value).Take(quantity).ToArray();
        }
    }
}
