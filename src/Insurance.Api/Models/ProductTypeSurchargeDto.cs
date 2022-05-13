namespace Insurance.Api.Models
{
    public class ProductTypeSurchargeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool CanBeInsured { get; set; }
        public float SurchargeRate { get; set; } = 0;
    }
}
