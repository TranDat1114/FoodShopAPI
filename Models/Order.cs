namespace FoodShopAPI;

public class Order : BaseEntity
{
    public string UserId { get; set; } = string.Empty;
    public string ProductId { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; }
    public EOrderStatus Status { get; set; }
    public DateTime OrderDate { get; set; }
}
