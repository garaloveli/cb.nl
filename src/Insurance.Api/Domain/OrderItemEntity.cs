namespace Insurance.Api.Domain
{
    public class OrderItemEntity
    {
        public ProductEntity Product { get; set; }
        public int Quantity { get; set; }

        public float InsuranceCost()
        {
            (int prodId, float insuranceVal) = Product.InsuranceCost();
            return (Quantity * insuranceVal) + (Product.ProductType.SurchargeRate ?? 0);
        }
    }
}