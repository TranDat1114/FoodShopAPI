namespace FoodShopAPI;

public class Product : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string CategoryId { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public int QuantityInStock { get; set; }
}
