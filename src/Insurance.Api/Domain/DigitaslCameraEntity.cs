namespace Insurance.Api.Domain
{
    public class DigitaslCameraEntity : ProductEntity
    {
        public DigitaslCameraEntity(int productId, int productTypeId, string productTypeName, float salesPrice, bool productTypeHasInsurance, float surchargeRate) : base(productId, productTypeId, productTypeName, salesPrice, productTypeHasInsurance, surchargeRate)
        {
        }
    }
}
