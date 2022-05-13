namespace Insurance.Api.Domain
{
    public record class ProductTypeEntity(
        int Id, 
        string Name, 
        bool CanBeInsured, 
        float? SurchargeRate = 0);
}
