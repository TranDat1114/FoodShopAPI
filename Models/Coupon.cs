namespace FoodShopAPI;

public class Coupon : BaseEntity
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public ECouponType CouponType { get; set; }
    public decimal MinimumAmount { get; set; }
    public decimal Discount { get; set; }
    public DateTime ExpiryDate { get; set; }
}
