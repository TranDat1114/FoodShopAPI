namespace FoodShopAPI;

public record UserLoginReqDTO
{
    public string EmailorUserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
public record UserLoginResDTO
{
    public string Token { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}
