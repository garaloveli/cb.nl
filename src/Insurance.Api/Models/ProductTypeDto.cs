namespace Insurance.Api.Models
{
    public readonly record struct ProductTypeDto(int Id, string Name, bool CanBeInsured);
}
