using Insurance.Api.Domain;
using Insurance.Api.Domain.Exceptions;
using Insurance.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Insurance.Tests.UnitTest
{
    public class ProductFactoryTest
    {
        #region Exceptions

        [Fact]
        public void FactoryException_GivenNullProduct_ShouldThrowException()
        {
            ProductDto? product = null;
            ProductTypeSurchargeDto productTypeSurcharge = new ProductTypeSurchargeDto();

            Assert.Throws<ProductNotFoundException>(() => ProductFactory.GetProduct(product, productTypeSurcharge));
        }

        [Fact]
        public void FactoryException_GivenEmptyProductProductTypeId_ShouldThrowException()
        {
            ProductDto? product = new ProductDto()
            {
                Id = 1,
                SalesPrice = 5f
            };
            ProductTypeSurchargeDto productTypeSurcharge = new ProductTypeSurchargeDto();
            Assert.Throws<ArgumentException>(() => ProductFactory.GetProduct(product, productTypeSurcharge));
        }

        [Fact]
        public void FactoryException_GivenNullType_ShouldThrowException()
        {
            ProductDto? product = new ProductDto()
            {
                Id = 1,
                ProductTypeId = 2,
                SalesPrice = 5f
            };
            ProductTypeSurchargeDto productTypeSurcharge = null;
            Assert.Throws<ProductTypeNotFoundException>(() => ProductFactory.GetProduct(product, productTypeSurcharge));
        }

        [Fact]
        public void FactoryException_GivenNonExistentType_ShouldThrowException()
        {
            ProductDto? product = new ProductDto()
            {
                Id = 1,
                ProductTypeId = 2,
                SalesPrice = 5f
            };
            ProductTypeSurchargeDto productTypeSurcharge = new ProductTypeSurchargeDto();
            Assert.Throws<ArgumentException>(() => ProductFactory.GetProduct(product, productTypeSurcharge));
        }
        #endregion

        [Fact]
        public void FactoryLaptop_GivenProductWithLaptopType_ShouldReturnLaptopEntity()
        {
            var product = new ProductDto(
                Id: (new Random()).Next(),
                Name: "ProdTest",
                ProductTypeId: TestConstants.LAPTOP_TYPE_ID,
                SalesPrice: 350);
            var prodTypes = new ProductTypeSurchargeDto {
                    Id = TestConstants.LAPTOP_TYPE_ID,
                    Name = "Test Type",
                    CanBeInsured = true
                };

            var result = ProductFactory.GetProduct(product, prodTypes);

            Assert.NotNull(result);
            Assert.IsType<LaptopEntity>(result);
        }

        [Fact]
        public void FactorySmartphone_GivenProductWithSmartphoneType_ShouldReturnSmartphoneEntity()
        {
            var product = new ProductDto(
                Id: (new Random()).Next(),
                Name: "ProdTest",
                ProductTypeId: TestConstants.SMARTPHONE_TYPE_ID,
                SalesPrice: 350);

            var prodTypes = new ProductTypeSurchargeDto {
                    Id = TestConstants.SMARTPHONE_TYPE_ID,
                    Name = "Test Type",
                    CanBeInsured = true };

            var result = ProductFactory.GetProduct(product, prodTypes);

            Assert.NotNull(result);
            Assert.IsType<SmartPhoneEntity>(result);
        }

        [Fact]
        public void FactoryProduct_GivenProductWithGeneralType_ShouldReturnProductEntity()
        {
            var product = new ProductDto(
                Id: (new Random()).Next(),
                Name: "ProdTest",
                ProductTypeId: 123123,
                SalesPrice: 350);
            var prodTypes = new ProductTypeSurchargeDto {
                    Id = 123123,
                    Name = "Test Type",
                    CanBeInsured = true
            };

            var result = ProductFactory.GetProduct(product, prodTypes);

            Assert.NotNull(result);
            Assert.IsType<ProductEntity>(result);
        }

        [Fact]
        public void FactoryCamera_GivenProductWithCameraType_ShouldReturnCameraEntity()
        {
            var product = new ProductDto(
                Id: (new Random()).Next(),
                Name: "ProdTest",
                ProductTypeId: TestConstants.DIGITAL_CAMERA_TYPE_ID,
                SalesPrice: 350);

            var prodTypes = new ProductTypeSurchargeDto
            {
                Id = TestConstants.DIGITAL_CAMERA_TYPE_ID,
                Name = "Test Type",
                CanBeInsured = true
            };

            var result = ProductFactory.GetProduct(product, prodTypes);

            Assert.NotNull(result);
            Assert.IsType<DigitaslCameraEntity>(result);
        }

    }
}
