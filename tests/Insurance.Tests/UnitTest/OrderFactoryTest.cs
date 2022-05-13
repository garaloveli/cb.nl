using Insurance.Api.Domain;
using Insurance.Api.Models;
using System.Collections.Generic;
using Xunit;

namespace Insurance.Tests.UnitTest
{
    public class OrderFactoryTest
    {
        [Fact]
        public void CalculateInsurance_GivenOneProductUnder500_ShouldNotAddInsurance()
        {
            OrderDto orderInput = new OrderDto {
                Items = new List<OrderItemDto>()
                {
                    new OrderItemDto(12345, 1)
                }
            };
            var products = new List<ProductDto?>{
                new ProductDto(
                    Id: 12345,
                    Name: "ProdTest",
                    ProductTypeId: TestConstants.LAPTOP_TYPE_ID,
                    SalesPrice: 350)
            };
            var prodTypes = new List<ProductTypeSurchargeDto> {
                new ProductTypeSurchargeDto{
                    Id = TestConstants.LAPTOP_TYPE_ID, 
                    Name = "Test Type", 
                    CanBeInsured = true
                }
            };

            var order = OrderFactory.GetOrder(orderInput, products, prodTypes);

            Assert.NotNull(order);
            Assert.IsType<OrderEntity>(order);
            Assert.Single(order.Items);
        }

        [Fact]
        public void CalculateInsurance_Given2ProductsMoreThan500LessThan2000_ShouldInsurance2000()
        {
            OrderDto orderInput = new OrderDto
            {
                Items = new List<OrderItemDto>()
                {
                    new OrderItemDto(12345, 1),
                    new OrderItemDto(12231, 1)
                }
            };
            var products = new List<ProductDto?>{
                new ProductDto(
                    Id: 12345,
                    Name: "ProdTest",
                    ProductTypeId: 4,
                    SalesPrice: 600),
                new ProductDto(
                    Id: 12231,
                    Name: "ProdTest",
                    ProductTypeId: 4,
                    SalesPrice: 1999)
            };
            var prodTypes = new List<ProductTypeSurchargeDto> {
                new ProductTypeSurchargeDto{
                    Id = 4,
                    Name = "Test Type",
                    CanBeInsured = true
                }
            };

            var order = OrderFactory.GetOrder(orderInput, products, prodTypes);
            var insuranceValue = order.InsuranceCost();

            Assert.Equal(2000, insuranceValue);
        }

        [Fact]
        public void CalculateInsurance_GivenCameraOver1000_ShouldInsurance4500()
        {
            OrderDto orderInput = new OrderDto
            {
                Items = new List<OrderItemDto>()
                {
                    new OrderItemDto(12345, 4)
                }
            };
            var products = new List<ProductDto?>{
                new ProductDto(
                    Id: 12345,
                    Name: "ProdTest",
                    ProductTypeId: TestConstants.DIGITAL_CAMERA_TYPE_ID,
                    SalesPrice: 600)
            };
            var prodTypes = new List<ProductTypeSurchargeDto> {
                new ProductTypeSurchargeDto{
                    Id = TestConstants.DIGITAL_CAMERA_TYPE_ID,
                    Name = "Test Type",
                    CanBeInsured = true
                }
            };

            var order = OrderFactory.GetOrder(orderInput, products, prodTypes);

            var insuranceValue = order.InsuranceCost();

            Assert.Equal(4500, insuranceValue);
        }
    }
}
