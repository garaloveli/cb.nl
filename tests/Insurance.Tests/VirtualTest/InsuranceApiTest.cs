using Insurance.Api.Controllers;
using Insurance.Api.Models;
using Insurance.Api.Gateways;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using Xunit;
using System.Threading.Tasks;

namespace Insurance.Tests.VirtualTest
{
    public class InsuranceApiTest
    {
        #region "Helper Methods"
        public HomeController GenerateHomeController()
        {
            return new HomeController(
                    new ProductGateway(
                        Options.Create(
                            new ProductAPIOptions()
                            {
                                Address = TestConstants.PRODUCT_API_URL
                            }),
                            NullLoggerFactory.Instance.CreateLogger<ProductGateway>()
                        ),
                    new ProductTypeGateway(
                        Options.Create(
                            new ProductAPIOptions()
                            {
                                Address = TestConstants.PRODUCT_API_URL
                            }),
                            NullLoggerFactory.Instance.CreateLogger<ProductTypeGateway>()
                        ),
                        NullLoggerFactory.Instance.CreateLogger<HomeController>()
                    );
        }
        #endregion

        [Fact]
        public async Task CalculateInsurance_ProductNotFound()
        {
            using (var host = VirtualTestHelper.MockHost(
                    (id) =>
                    {
                        return null;
                    }, () =>
                    {
                        return new List<ProductTypeDto>();
                    }, (typeId) =>
                    {
                        return null;
                    }))
            {

                var dto = new InsuranceRequestDto
                {
                    ProductId = 1,
                };

                var sut = GenerateHomeController();

                var result = await sut.CalculateInsurance(dto);
                Assert.IsType<NotFoundResult>(result.Result);
            }
        }

        [Fact]
        public void CalculateInsurance_GivenSalesPriceBetween500And2000Euros_ShouldAddThousandEurosToInsuranceCost()
        {
            using (var host = VirtualTestHelper.MockHost(
                (id) =>
                {
                    return new ProductDto()
                    {
                        Id = 1,
                        SalesPrice = 750f,
                        ProductTypeId = 1,
                    };
                }, () =>
                {
                    return new List<ProductTypeDto>() {
                        new ProductTypeDto() {
                             Id=1, CanBeInsured = true, Name = "Test type"
                        }
                    };
                }, (typeId) =>
                {
                    return new ProductTypeDto()
                    {
                        Id = 1,
                        CanBeInsured = true,
                        Name = "Test type"
                    };
                }))
            {

                const float expectedInsuranceValue = 1000;

                var dto = new InsuranceRequestDto
                {
                    ProductId = 1,
                };

                var homeController = GenerateHomeController();

                var result = homeController.CalculateInsurance(dto).Result;

                Assert.Equal(
                    expected: expectedInsuranceValue,
                    actual: result.Value.InsuranceValue
                );
            }
        }

        [Fact]
        public void CalculateInsurance_GivenOrderWith2Items_ShouldReturnSumInsurance()
        {
            using (var host = VirtualTestHelper.MockHost(
                (id) =>
                {
                    switch (id)
                    {
                        case 1:
                            return new ProductDto()
                            {
                                Id = 1,
                                SalesPrice = 750f,
                                ProductTypeId = 1,
                            };
                        case 2:
                            return new ProductDto()
                            {
                                Id = 2,
                                SalesPrice = 350f,
                                ProductTypeId = TestConstants.LAPTOP_TYPE_ID,
                            };
                        default:
                            return new ProductDto()
                            {
                                Id = id,
                                SalesPrice = 100f,
                                ProductTypeId = 1
                            };
                    }

                }, () =>
                {
                    return new List<ProductTypeDto>() {
                        new ProductTypeDto() {
                             Id=1, CanBeInsured = true, Name = "Test type" },
                        new ProductTypeDto() {
                             Id=21, CanBeInsured = true, Name = "Test type2"
                        }
                    };
                }, (typeId) =>
                {
                    return new ProductTypeDto()
                    {
                        Id = typeId,
                        CanBeInsured = true,
                        Name = "Test Product Type"
                    };
                }))
            {
                var controller = GenerateHomeController();

                var request = new OrderDto()
                {
                    Items = new List<OrderItemDto>
                    {
                        new OrderItemDto(ProductId : 1, Quantity : 1),
                        new OrderItemDto(ProductId : 2, Quantity : 3)
                    }
                };

                var res = controller.CalculateOrderInsurance(request).Result;

                Assert.NotNull(res);
                Assert.Equal(
                    expected: 2500,
                    actual: res.Value.InsuranceValue);
            }
        }

        [Fact]
        public void CalculateInsurance_GivenOrderWith2ItemsMoreThan500LessThan2000_ShouldInsurance2000()
        {
            using (var host = VirtualTestHelper.MockHost(
                (id) =>
                {
                    return new ProductDto()
                    {
                        Id = id,
                        SalesPrice = 750f,
                        ProductTypeId = 1
                    };

                }, () =>
                {
                    return new List<ProductTypeDto>() {
                        new ProductTypeDto() {
                            Id = 1, 
                            CanBeInsured = true, 
                            Name = "Test type" }
                    };
                }, (typeId) =>
                {
                    return new ProductTypeDto()
                    {
                        Id = typeId,
                        CanBeInsured = true,
                        Name = "Test Product Type"
                    };
                }))
            {
                var controller = GenerateHomeController();

                var request = new OrderDto()
                {
                    Items = new List<OrderItemDto>
                    {
                        new OrderItemDto(ProductId : 1, Quantity : 1),
                        new OrderItemDto(ProductId : 2, Quantity : 1)
                    }
                };

                var res = controller.CalculateOrderInsurance(request).Result;

                Assert.NotNull(res);
                Assert.Equal(
                    expected: 2000,
                    actual: res.Value.InsuranceValue);
            }
        }
    }
}
