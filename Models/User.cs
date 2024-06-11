using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace FoodShopAPI;

public class User : IdentityUser
{
    public string Address { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Avatar { get; set; } = string.Empty;
    [NotMapped]
    public string Role { get; set; } = string.Empty;
}

