namespace FoodShopAPI;

public class Comment : BaseEntity
{
    public string UserId { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? ReviewId { get; set; }
}
