namespace Insurance.Api.Domain
{
    public class LaptopEntity : ProductEntity
    {
        public LaptopEntity(int productId, int productTypeId, string productTypeName, float salesPrice, bool productTypeHasInsurance, float surchargeRate) : 
            base(productId, productTypeId, productTypeName, salesPrice, productTypeHasInsurance, surchargeRate)
        {
        }

        public override (int productId, float insuranceValue) InsuranceCost()
        {
            (int tmpProductId, float subInsuranceValue) = base.InsuranceCost();

            subInsuranceValue += 500;

            return (tmpProductId, subInsuranceValue);
        }
    }
}
