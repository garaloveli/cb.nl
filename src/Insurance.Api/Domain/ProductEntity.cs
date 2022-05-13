using Insurance.Api.Domain;

namespace Insurance.Api.Domain
{
    public class ProductEntity
    {
        public int ProductId { get; set; }

        public ProductTypeEntity ProductType { get; protected set; }
        public float SalesPrice { get; protected set; }

        public ProductEntity(int productId,
            int productTypeId,
            string productTypeName,
            float salesPrice,
            bool productTypeHasInsurance,
            float surchargeRate)
        {
            this.ProductId = productId;
            this.ProductType = new ProductTypeEntity(Id: productTypeId, Name: productTypeName, CanBeInsured: productTypeHasInsurance, SurchargeRate: surchargeRate);
            this.SalesPrice = salesPrice;
        }

        public virtual (int productId, float insuranceValue) InsuranceCost()
        {
            var insurance = 0;
            if (this.ProductType.CanBeInsured)
            {
                if (this.SalesPrice >= 500 && this.SalesPrice < 2000)
                {
                    insurance += 1000;
                }
                else if (this.SalesPrice >= 2000)
                {
                    insurance += 2000;
                }
            }

            return (this.ProductId, insurance);
        }
    }
}
