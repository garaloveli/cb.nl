using Insurance.Api.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Insurance.Tests.UnitTest
{
    public class ProductEntityTest
    {
        // If the product sales price is less than €500, no insurance required.
        [Fact]
        public void CalculateInsurance_GivenProductPriceUnder500_ShouldNoInsurance()
        {
            var product = new ProductEntity(
                productId: 54353, 
                productTypeId: 1, 
                productTypeName: "test", 
                salesPrice: 400, 
                productTypeHasInsurance: true,
                surchargeRate: 0);
            
            var result = product.InsuranceCost();
            
            Assert.Equal(product.ProductId, result.productId);
            Assert.Equal(0, result.insuranceValue);
        }

        [Fact]
        public void CalculateInsurance_GivenProductPrice500_ShouldInsuranceCostBe1000()
        {
            var product = new ProductEntity(
                productId: (new Random()).Next(),
                productTypeId: 1,
                productTypeName: "Test TypeName",
                salesPrice: 500,
                productTypeHasInsurance: true,
                surchargeRate: 0);

            var result = product.InsuranceCost();

            Assert.Equal(product.ProductId, result.productId);
            Assert.Equal(1000, result.insuranceValue);
        }

        // If the product sales price => €500 but < €2000, insurance cost is €1000.
        [Fact]
        public void CalculateInsurance_GivenPriceBetween500And2000_ShouldInsuranceCostBe1000()
        {
            var product = new ProductEntity(
                productId: 4563,
                productTypeId: 1,
                productTypeName: "test",
                salesPrice: 600,
                productTypeHasInsurance: true,
                surchargeRate: 0);

            var result = product.InsuranceCost();

            Assert.Equal(product.ProductId, result.productId);
            Assert.Equal(1000, result.insuranceValue);
        }

        [Fact]
        public void CalculateInsurance_GivenProductPrice2000_ShouldInsuranceCostBe2000()
        {
            var product = new ProductEntity(
                productId: (new Random()).Next(),
                productTypeId: 1,
                productTypeName: "Test TypeName",
                salesPrice: 2000,
                productTypeHasInsurance: true,
                surchargeRate: 0);

            var result = product.InsuranceCost();

            Assert.Equal(product.ProductId, result.productId);
            Assert.Equal(2000, result.insuranceValue);
        }

        // If the product sales price => €2000, insurance cost is €2000.
        [Fact]
        public void CalculateInsurance_GivenPriceGreaterThan2000_ShouldInsurancePriceBe2000()
        {
            var product = new ProductEntity(
                productId: 9876,
                productTypeId: 1,
                productTypeName: "test",
                salesPrice: 2000,
                productTypeHasInsurance: true,
                surchargeRate: 0);

            var result = product.InsuranceCost();

            Assert.Equal(product.ProductId, result.productId);
            Assert.Equal(2000, result.insuranceValue);
        }

        // If the type of the product is a smartphone or a laptop, add €500 more to the insurance cost.
        // TODO Task1: The financial manager reportred that when customers buy a laptop that costs less than €500, insurance is not calculated, while it should be €500.
        [Fact]
        public void CalculateInsurance_GivenLaptopTypeUnder500_ShouldInsurancePriceBe500()
        {
            var product = new LaptopEntity(
                productId: 21231, 
                productTypeId: TestConstants.LAPTOP_TYPE_ID, 
                productTypeName: "Test Laptops", 
                salesPrice: 300, 
                productTypeHasInsurance: true,
                surchargeRate: 0);

            var result = product.InsuranceCost();
            
            Assert.Equal(product.ProductId, result.productId);
            Assert.Equal(500, result.insuranceValue);
        }

        [Fact]
        public void CalculateInsurance_GivenSmartPhoneUnder500_ShouldInsurancePriceBe500()
        {
            var product = new SmartPhoneEntity(
                productId: 65476,
                productTypeId: TestConstants.SMARTPHONE_TYPE_ID,
                productTypeName: "Test Smartphone",
                salesPrice: 356,
                productTypeHasInsurance: true,
                surchargeRate: 0);

            var result = product.InsuranceCost();

            Assert.Equal(product.ProductId, result.productId);
            Assert.Equal(500, result.insuranceValue);
        }

        [Fact]
        public void CalculateInsurance_GivenLaptopPriceOver2000_ShouldInsurancePriceBe2500()
        {
            var product = new LaptopEntity(
                productId: 654890,
                productTypeId: TestConstants.LAPTOP_TYPE_ID,
                productTypeName: "Test Laptop",
                salesPrice: 2300,
                productTypeHasInsurance: true,
                surchargeRate: 0);

            var result = product.InsuranceCost();

            Assert.Equal(product.ProductId, result.productId);
            Assert.Equal(2500, result.insuranceValue);
        }
    }
}
