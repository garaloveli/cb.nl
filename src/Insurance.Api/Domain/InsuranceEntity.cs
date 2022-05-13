using Insurance.Api.Domain;

namespace Insurance.Api.Domain
{
    public class InsuranceEntity
    {
        private ProductEntity product;
        private float InsuranceValue = 0;
        public InsuranceEntity(ProductEntity product)
        {
            this.product = product;
        }

        
    }
}
