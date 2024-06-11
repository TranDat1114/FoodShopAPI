using Microsoft.AspNetCore.Identity;

namespace FoodShopAPI;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this WebApplication app)
    {
        var UserGroup = app.MapGroup("users").WithTags("Users");
        UserGroup.MapPost("register", async (UserRegisterReqDTO userRegisterDTO, UserManager<User> userManager) =>
        {
            var user = new User
            {
                Email = userRegisterDTO.Email,
                UserName = userRegisterDTO.UserName,
                PhoneNumber = userRegisterDTO.PhoneNumber,
                Role = SD.User
            };
            var result = await userManager.CreateAsync(user, userRegisterDTO.Password);
            if (!result.Succeeded)
            {
                return Results.BadRequest(result.Errors);
            }
            return Results.Ok(
                new UserRegisterResDTO
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    Role = user.Role,
                    PhoneNumber = user.PhoneNumber
                }
            );
        });

        UserGroup.MapPost("login", async (UserLoginReqDTO userLoginDTO, UserManager<User> userManager, IJwtService jwtService) =>
        {
            var user = await userManager.FindByEmailAsync(userLoginDTO.EmailorUserName) ?? await userManager.FindByNameAsync(userLoginDTO.EmailorUserName);
            if (user == null)
            {
                return Results.BadRequest("Invalid Email");
            }
            if (!await userManager.CheckPasswordAsync(user, userLoginDTO.Password))
            {
                return Results.BadRequest("Invalid Password");
            }
            var role = userManager.GetRolesAsync(user).Result.First();
            var jwt = jwtService.GenerateToken(user.Id, user.UserName ?? "", user.Email ?? "", role);

            return Results.Ok(
                new UserLoginResDTO
                {
                    Token = jwt,
                    UserName = user.UserName ?? "",
                    Email = user.Email ?? "",
                    Role = role
                }
            );
        });
    }

}
