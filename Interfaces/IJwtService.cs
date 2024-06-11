namespace FoodShopAPI;

public interface IJwtService
{
    string GenerateToken(string userId, string userName, string Email, string role);
}
