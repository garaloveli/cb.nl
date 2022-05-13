using Insurance.Api.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Insurance.Tests.UnitTest
{
    public class OrderEntityTest
    {
        [Fact]
        public void CalculateOrderInsurance_Given2ItemsPriceGreaterEqualThan500_ShouldInsuranceCost2000()
        {
            var order = new OrderEntity {
                Items = new List<OrderItemEntity> { 
                    new OrderItemEntity {
                        Quantity = 1,
                        Product = new ProductEntity(
                            productId: 54353,
                            productTypeId: 1,
                            productTypeName: "test1",
                            salesPrice: 501,
                            productTypeHasInsurance: true,
                            surchargeRate: 0) },
                    new OrderItemEntity {
                        Quantity = 1,
                        Product = new ProductEntity(
                            productId: 67538,
                            productTypeId: 2,
                            productTypeName: "test2",
                            salesPrice: 500,
                            productTypeHasInsurance: true,
                            surchargeRate: 0) }
                }
            };

            var insuranceCost = order.InsuranceCost();

            Assert.Equal(2000, insuranceCost);
        }

        [Fact]
        public void CalculateOrderInsurance_Given2CamerasUnder500_ShouldInsuranceCost500()
        {
            var order = new OrderEntity
            {
                Items = new List<OrderItemEntity> {
                    new OrderItemEntity {
                        Quantity = 2,
                        Product = new DigitaslCameraEntity(
                            productId: 54323,
                            productTypeId: TestConstants.DIGITAL_CAMERA_TYPE_ID,
                            productTypeName: "test",
                            salesPrice: 499,
                            productTypeHasInsurance: true,
                            surchargeRate: 0) }
                }
            };

            var insuranceCost = order.InsuranceCost();

            Assert.Equal(500, insuranceCost);
        }

        [Fact]
        public void CalculateOrderInsuranceWithSurcharge_Given2ProductsUnder500With400Surcharge_ShouldInsuranceCost400()
        {
            var order = new OrderEntity
            {
                Items = new List<OrderItemEntity> {
                    new OrderItemEntity {
                        Quantity = 2,
                        Product = new ProductEntity(
                            productId: 54323,
                            productTypeId: 1,
                            productTypeName: "test",
                            salesPrice: 499,
                            productTypeHasInsurance: true,
                            surchargeRate: 400) }
                }
            };

            var insuranceCost = order.InsuranceCost();

            Assert.Equal(400, insuranceCost);
        }
    }
}
