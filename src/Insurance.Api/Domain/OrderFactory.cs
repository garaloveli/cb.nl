using Insurance.Api.Models;
using System.Collections.Generic;
using System.Linq;

namespace Insurance.Api.Domain
{
    public class OrderFactory
    {
        public static OrderEntity GetOrder(OrderDto order, IEnumerable<ProductDto?> products, IEnumerable<ProductTypeSurchargeDto> types)
        {
            var orderEntity = new OrderEntity();
            var nonEmptyProducts = products.Where(x => x.HasValue).Select(x => x.Value).ToList();
            var cleanedProductTypes = types.DistinctBy(t => t.Id).ToList();
            foreach (var item in order.Items)
            {
                var productDto = nonEmptyProducts.Where(x => x.Id == item.ProductId).Single();
                var prodTypeDto = cleanedProductTypes.Where(x => x.Id == productDto.ProductTypeId).Single();

                var orderItemProduct = ProductFactory.GetProduct(productDto, prodTypeDto);

                orderEntity.Items.Add(new OrderItemEntity()
                {
                    Product = orderItemProduct,
                    Quantity = item.Quantity
                });
            }
            return orderEntity;
        }
    }
}
