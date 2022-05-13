using Insurance.Api.Domain.Exceptions;
using Insurance.Api.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
//using System.Threading;
using System.Threading.Tasks;

namespace Insurance.Api.Gateways
{
    public class ProductTypeGateway : IProductTypeGateway
    {
        private ILogger _logger;
        private ProductAPIOptions options;
        private volatile IDictionary<int, ProductTypeSurchargeDto> state = new ConcurrentDictionary<int, ProductTypeSurchargeDto>();
        public ProductTypeGateway(IOptions<ProductAPIOptions> options,
            ILogger<ProductTypeGateway> logger)
        {
            this._logger = logger;
            this.options = options.Value;
            GetProductTypes();
        }

        private SemaphoreSlim stateLock = new SemaphoreSlim(1);
        public async Task<ProductTypeSurchargeDto?> GetProductType(int productTypeId)
        {
            if (state.ContainsKey(productTypeId))
            {
                return state[productTypeId];
            }
            else
            {
                await stateLock.WaitAsync();
                try
                {
                    if (state.ContainsKey(productTypeId))
                    {
                        return state[productTypeId];
                    }
                    else
                    {
                        var prodType = await InternalGetProductType(productTypeId);
                        if (prodType == null) return null;
                        state[productTypeId] = new ProductTypeSurchargeDto
                        {
                            Id = prodType.Value.Id,
                            Name = prodType.Value.Name,
                            CanBeInsured = prodType.Value.CanBeInsured
                        };
                        return state[productTypeId];
                    }
                }
                finally
                {
                    stateLock.Release();
                }
            }
        }
        private async Task<ProductTypeDto?> InternalGetProductType(int productTypeId)
        {
            HttpClient client = new HttpClient
            {
                BaseAddress = new Uri(options.Address)
            };
            var response = await client.GetAsync(string.Format("/product_types/{0:G}", productTypeId));
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    var json = await response.Content.ReadAsStringAsync();
                    if (string.IsNullOrEmpty(json))
                    {
                        return null;
                    }
                    var productType = JsonConvert.DeserializeObject<ProductTypeDto>(json);
                    return productType;
                case HttpStatusCode.NotFound:
                    return null;
                default:
                    throw new ApplicationException($" { response.StatusCode } ");
            }
        }

        private void GetProductTypes()
        {
            HttpClient client = new HttpClient
            {
                BaseAddress = new Uri(options.Address)
            };
            string json = client.GetAsync("/product_types").Result.Content.ReadAsStringAsync().Result;
            var collection = JsonConvert.DeserializeObject<IEnumerable<ProductTypeDto>>(json);

            state.Clear();
            foreach (var item in collection)
            {
                state.Add(item.Id, new ProductTypeSurchargeDto {
                    Id = item.Id,
                    Name = item.Name,
                    CanBeInsured = item.CanBeInsured,
                    SurchargeRate = 0,
                });
            }
        }
        
        public float? GetSurchargeRate(int productTypeId)
        {
            if (!state.ContainsKey(productTypeId))
            {
                return null;
            }

            return state[productTypeId].SurchargeRate;
        }

        private SemaphoreSlim surchargeStateLock = new SemaphoreSlim(1);
        public async Task SaveSurchargeRate(int productTypeId, float surchargeRate)
        {
            await surchargeStateLock.WaitAsync();
            try
            {
                if (state.ContainsKey(productTypeId))
                {
                    state[productTypeId].SurchargeRate = surchargeRate;
                }
                else
                {
                    var prodType = await InternalGetProductType(productTypeId);
                    if (prodType == null) throw new ProductTypeNotFoundException();
                    state[productTypeId] = new ProductTypeSurchargeDto
                    {
                        Id = prodType.Value.Id,
                        Name = prodType.Value.Name,
                        CanBeInsured = prodType.Value.CanBeInsured,
                        SurchargeRate = surchargeRate
                    };
                }
            }
            finally
            {
                surchargeStateLock.Release();
            }
        }
    }
}
