using Insurance.Api.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Tests.VirtualTest
{
    public class ControllerTestStartup
    {
        private Func<int, ProductDto?> _mockProduct;
        private Func<IEnumerable<ProductTypeDto>> _mockTypes;
        private Func<int, ProductTypeDto?> _mockProductType;
        public ControllerTestStartup(
            Func<int, ProductDto?> mockProduct,
            Func<IEnumerable<ProductTypeDto>> mockTypes,
            Func<int, ProductTypeDto?> mockProductType)
        {
            _mockProduct = mockProduct;
            _mockTypes = mockTypes;
            _mockProductType = mockProductType;
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseEndpoints(
                ep =>
                {
                    ep.MapGet(
                        "products/{id:int}",
                        context =>
                        {
                            int productId = int.Parse((string)context.Request.RouteValues["id"]);
                            var product = _mockProduct(productId);
                            
                            if (product != null)
                            {
                                return context.Response.WriteAsync(JsonConvert.SerializeObject(product));
                            }
                            else
                            {
                                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                                return context.Response.WriteAsync("");
                            }
                        }
                    );
                    ep.MapGet(
                        "product_types",
                        context =>
                        {
                            return context.Response.WriteAsync(JsonConvert.SerializeObject(_mockTypes()));
                        }
                    );
                    ep.MapGet(
                        "product_types/{id:int}",
                        context =>
                        {
                            int productTypeId = int.Parse((string)context.Request.RouteValues["id"]);
                            var productType = _mockProductType(productTypeId);
                            if (productType != null)
                            {
                                return context.Response.WriteAsync(JsonConvert.SerializeObject(productType));
                            }
                            else
                            {
                                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                                return context.Response.WriteAsync("");
                            }
                        }
                    );
                }
            );
        }
    }

    internal class VirtualTestHelper
    {
        internal static IHost MockHost(
            Func<int, ProductDto?> mockProduct, 
            Func<IEnumerable<ProductTypeDto>> mockTypes,
            Func<int, ProductTypeDto?> mockProductType,
            string address = TestConstants.PRODUCT_API_URL)
        {
            var _host = new HostBuilder()
                   .ConfigureWebHostDefaults(
                        b => {
                            b.UseUrls(address).UseStartup(
                                (c) => new ControllerTestStartup(mockProduct, mockTypes, mockProductType));
                        }
                    )
                   .Build();

            _host.Start();
            return _host;
        }

    }
}