using Insurance.Api.Models;
using Insurance.Api.Domain.Exceptions;
using System;
using System.Collections.Generic;
using Insurance.Api.Domain.Enums;
using System.Linq;

namespace Insurance.Api.Domain
{
    public class ProductFactory
    {
        private static void ValidateProduct(ProductDto? productPayload)
        {
            if (!productPayload.HasValue)
            {
                throw new ProductNotFoundException();
            }
            else if (!productPayload.Value.ProductTypeId.HasValue)
            {
                throw new ArgumentException(nameof(productPayload.Value.ProductTypeId));
            }
        }

        private static void ValidateProducType(ProductTypeSurchargeDto? productType) {
            if (productType == null)
            {
                throw new ProductTypeNotFoundException();
            }
        }

        private static void Validate(ProductDto? productPayload, ProductTypeSurchargeDto? productType)
        {
            if (productPayload.Value.ProductTypeId != productType.Id)
            {
                throw new ArgumentException(nameof(productPayload.Value.ProductTypeId));
            }
        }

        private static ProductEntity InternalGet(ProductDto productPayload, ProductTypeSurchargeDto productType)
        {
            

            string typeName = productType.Name;
            bool hasInsurance = productType.CanBeInsured;

            ProductEntity product;

            switch (productPayload.ProductTypeId)
            {
                case (int)ProductTypeEnum.Laptops:
                    product = new LaptopEntity(
                        productId: productPayload.Id.GetValueOrDefault(),
                        productTypeId: productPayload.ProductTypeId.Value,
                        productTypeName: typeName,
                        salesPrice: productPayload.SalesPrice.GetValueOrDefault(),
                        productTypeHasInsurance: hasInsurance,
                        surchargeRate: productType.SurchargeRate);
                    break;

                case (int)ProductTypeEnum.Smartphones:
                    product = new SmartPhoneEntity(
                        productId: productPayload.Id.GetValueOrDefault(),
                        productTypeId: productPayload.ProductTypeId.Value,
                        productTypeName: typeName,
                        salesPrice: productPayload.SalesPrice.GetValueOrDefault(),
                        productTypeHasInsurance: hasInsurance,
                        surchargeRate: productType.SurchargeRate);
                    break;

                case (int)ProductTypeEnum.DigitalCameras:
                    product = new DigitaslCameraEntity(
                        productId: productPayload.Id.GetValueOrDefault(),
                        productTypeId: productPayload.ProductTypeId.Value,
                        productTypeName: typeName,
                        salesPrice: productPayload.SalesPrice.GetValueOrDefault(),
                        productTypeHasInsurance: hasInsurance,
                        surchargeRate: productType.SurchargeRate);
                    break;

                default:
                    product = new ProductEntity(
                        productId: productPayload.Id.GetValueOrDefault(),
                        productTypeId: productPayload.ProductTypeId.Value,
                        productTypeName: typeName,
                        salesPrice: productPayload.SalesPrice.GetValueOrDefault(),
                        productTypeHasInsurance:hasInsurance,
                        surchargeRate: productType.SurchargeRate);
                    break;
            }

            return product;
        }

        public static ProductEntity GetProduct(ProductDto? productPayload, ProductTypeSurchargeDto? productType)
        {
            ValidateProduct(productPayload);
            ValidateProducType(productType);
            Validate(productPayload, productType);
            return InternalGet(productPayload.GetValueOrDefault(), productType);
        }
    }
}
