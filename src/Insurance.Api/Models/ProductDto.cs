namespace Insurance.Api.Models
{
    public readonly record struct ProductDto(int? Id, string Name, int? ProductTypeId, float? SalesPrice);
}
