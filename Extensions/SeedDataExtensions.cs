using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FoodShopAPI;

public static class SeedDataExtensions
{

    public static async Task ApplySeedDataAsync(this IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
        using var context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var serviceProvider = serviceScope.ServiceProvider;
        var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
        try
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            await SeedUserAsync(userManager, roleManager, context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred seeding the DB.");
        }
    }
    public static async Task SeedUserAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
    {
        if (!await roleManager.RoleExistsAsync(SD.Admin.ToString()))
        {
            await roleManager.CreateAsync(new IdentityRole(SD.Admin));
            await roleManager.CreateAsync(new IdentityRole(SD.User));
            await roleManager.CreateAsync(new IdentityRole(SD.Manager));
            // Ensure the Admin user exists

            var adminEmail = "admin@example.com";
            var adminUser = new User { UserName = "admin", Email = adminEmail, EmailConfirmed = true, PhoneNumber = "1234567890", PhoneNumberConfirmed = true };
            var user = await userManager.FindByEmailAsync(adminEmail);

            if (user == null)
            {
                var createAdminUser = await userManager.CreateAsync(adminUser, "Admin@123456");
                if (createAdminUser.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, SD.Admin);
                }
            }

            var demoUserEmail = "demo@gmail.com";
            var demoUser = new User { UserName = "demouser", Email = demoUserEmail, EmailConfirmed = true, PhoneNumber = "0987654321", PhoneNumberConfirmed = true };
            var demoUserExist = await userManager.FindByEmailAsync(demoUserEmail);

            if (demoUserExist == null)
            {
                var createDemoUser = await userManager.CreateAsync(demoUser, "Demo@123456");
                if (createDemoUser.Succeeded)
                {
                    await userManager.AddToRoleAsync(demoUser, SD.User);
                }
            }

            await SeedDataAsync(context);
            // await SeedRelationShipDataAsync(context);
        }
    }

    public static async Task SeedDataAsync(ApplicationDbContext context)
    {
        if (!context.ProductCategories.AsNoTracking().Any())
        {
            var productCategories = new List<ProductCategory>
            {
                new() { Name = "Fruit", Description = "Fresh fruit" },
                new() { Name = "Pizza", Description = "Great pizza" },
                new() { Name = "Vegetable", Description = "Fresh vegetable" },
                new() { Name = "Meat", Description = "Fresh meat" },
                new() { Name = "Seafood", Description = "Fresh seafood" }
            };

            await context.ProductCategories.AddRangeAsync(productCategories);
            await context.SaveChangesAsync();
        }

        if (!context.Products.AsNoTracking().Any())
        {
            var fruitProductCategory = await context.ProductCategories
            .AsNoTracking()
            .FirstAsync(x => x.Name == "Fruit");
            var pizzaProductCategory = await context.ProductCategories
            .AsNoTracking()
            .FirstAsync(x => x.Name == "Pizza");
            var vegetableProductCategory = await context.ProductCategories
            .AsNoTracking()
            .FirstAsync(x => x.Name == "Vegetable");
            var meatProductCategory = await context.ProductCategories
            .AsNoTracking()
            .FirstAsync(x => x.Name == "Meat");
            var seafoodProductCategory = await context.ProductCategories
            .AsNoTracking()
            .FirstAsync(x => x.Name == "Seafood");

            var listFruitProduct = new List<Product>
            {
                new() {
                    Name = "Apple",
                    Description = "Fresh apple",
                    Price = 1.99m,
                    ImageUrl = "https://images.unsplash.com/photo-1570913149827-d2ac84ab3f9a?q=80&w=1770&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D",
                    QuantityInStock = 100,
                    CategoryId = fruitProductCategory.Guid
                },
                new() {
                    Name = "Banana",
                    Description = "Fresh banana",
                    Price = 0.99m,
                    ImageUrl = "https://images.unsplash.com/photo-1528825871115-3581a5387919?q=80&w=1915&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D",
                    QuantityInStock = 20,
                    CategoryId = fruitProductCategory.Guid
                },
                new() {
                    Name = "Orange",
                    Description = "Fresh orange",
                    Price = 1.49m,
                    ImageUrl = "https://images.unsplash.com/photo-1557800636-894a64c1696f?q=80&w=1965&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D",
                    QuantityInStock = 200,
                    CategoryId = fruitProductCategory.Guid
                },
                new() {
                    Name = "Pineapple",
                    Description = "Fresh pineapple",
                    Price = 2.49m,
                    ImageUrl = "https://images.unsplash.com/photo-1589820296156-2454bb8a6ad1?q=80&w=1887&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D",
                    QuantityInStock = 69,
                    CategoryId = fruitProductCategory.Guid
                },
                new() {
                    Name = "Strawberry",
                    Description = "Fresh strawberry",
                    Price = 3.49m,
                    ImageUrl = "https://images.unsplash.com/photo-1588165171080-c89acfa5ee83?q=80&w=1887&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D",
                    QuantityInStock = 96,
                    CategoryId = fruitProductCategory.Guid
                },
                new() {
                    Name = "Margherita",
                    Description = "Tomato sauce, mozzarella, fresh basil",
                    Price = 9.99m,
                    ImageUrl = "https://images.unsplash.com/photo-1574071318508-1cdbab80d002?q=80&w=1769&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D",
                    QuantityInStock = 92,
                    CategoryId = pizzaProductCategory.Guid
                },
                new() {
                    Name = "Pepperoni",
                    Description = "Tomato sauce, mozzarella, pepperoni",
                    Price = 10.99m,
                    ImageUrl = "https://images.unsplash.com/photo-1628840042765-356cda07504e?q=80&w=1780&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D",
                    QuantityInStock = 24,
                    CategoryId = pizzaProductCategory.Guid
                },
                new() {
                    Name = "Vegetarian",
                    Description = "Tomato sauce, mozzarella, mushroom, onion, bell pepper",
                    Price = 11.99m,
                    ImageUrl = "https://plus.unsplash.com/premium_photo-1690056321981-dfe9e75e0247?q=80&w=1887&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D",
                    QuantityInStock = 11,
                    CategoryId = pizzaProductCategory.Guid
                },
                new() {
                    Name = "Hawaiian",
                    Description = "Tomato sauce, mozzarella, ham, pineapple",
                    Price = 12.99m,
                    ImageUrl = "https://images.unsplash.com/photo-1590534247854-e97d5e3feef6?q=80&w=1770&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D",
                    QuantityInStock = 69,
                    CategoryId = pizzaProductCategory.Guid
                },
                new() {
                    Name = "Meat Lovers",
                    Description = "Tomato sauce, mozzarella, pepperoni, sausage, bacon",
                    Price = 13.99m,
                    ImageUrl = "https://images.unsplash.com/photo-1513104890138-7c749659a591?q=80&w=1770&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D",
                    QuantityInStock = 97,
                    CategoryId = pizzaProductCategory.Guid
                },
                new() {
                    Name = "Carrot",
                    Description = "Fresh carrot",
                    Price = 0.99m,
                    ImageUrl = "https://images.unsplash.com/photo-1598170845058-32b9d6a5da37?q=80&w=1887&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D",
                    QuantityInStock = 100,
                    CategoryId = vegetableProductCategory.Guid
                },
                new() {
                    Name = "Cucumber",
                    Description = "Fresh cucumber",
                    Price = 1.49m,
                    ImageUrl = "https://images.unsplash.com/photo-1449300079323-02e209d9d3a6?q=80&w=1974&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D",
                    QuantityInStock = 200,
                    CategoryId = vegetableProductCategory.Guid
                },
                new() {
                    Name = "Tomato",
                    Description = "Fresh tomato",
                    Price = 2.49m,
                    ImageUrl = "https://images.unsplash.com/photo-1607305387299-a3d9611cd469?q=80&w=1770&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D",
                    QuantityInStock = 69,
                    CategoryId = vegetableProductCategory.Guid
                },
                new() {
                    Name = "Potato",
                    Description = "Fresh potato",
                    Price = 3.49m,
                    ImageUrl = "https://images.unsplash.com/photo-1518977676601-b53f82aba655?q=80&w=1770&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D",
                    QuantityInStock = 96,
                    CategoryId = vegetableProductCategory.Guid
                },
                new() {
                    Name = "Onion",
                    Description = "Fresh onion",
                    Price = 4.49m,
                    ImageUrl = "https://images.unsplash.com/photo-1587049633312-d628ae50a8ae?q=80&w=1780&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D",
                    QuantityInStock = 96,
                    CategoryId = vegetableProductCategory.Guid
                },
                new() {
                    Name = "Beef",
                    Description = "Fresh beef",
                    Price = 5.99m,
                    ImageUrl = "https://images.unsplash.com/photo-1615937691194-97dbd3f3dc29?q=80&w=1770&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D",
                    QuantityInStock = 100,
                    CategoryId = meatProductCategory.Guid
                },
                new() {
                    Name = "Chicken",
                    Description = "Fresh chicken",
                    Price = 6.49m,
                    ImageUrl = "https://images.unsplash.com/photo-1587593810167-a84920ea0781?q=80&w=1770&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D",
                    QuantityInStock = 200,
                    CategoryId = meatProductCategory.Guid
                },
                new() {
                    Name = "Pork",
                    Description = "Fresh pork",
                    Price = 7.49m,
                    ImageUrl = "https://images.unsplash.com/photo-1560781290-7dc94c0f8f4f?q=80&w=1935&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D",
                    QuantityInStock = 69,
                    CategoryId = meatProductCategory.Guid
                },
                new() {
                    Name = "Lamb",
                    Description = "Fresh lamb",
                    Price = 8.49m,
                    ImageUrl = "https://plus.unsplash.com/premium_photo-1666620504958-4056c0def05b?q=80&w=1887&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D",
                    QuantityInStock = 96,
                    CategoryId = meatProductCategory.Guid
                },
                new() {
                    Name = "Turkey",
                    Description = "Cooked turkey",
                    Price = 9.49m,
                    ImageUrl = "https://images.unsplash.com/photo-1617636423451-0db0119c14cd?q=80&w=1771&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D",
                    QuantityInStock = 96,
                    CategoryId = meatProductCategory.Guid
                },
                new() {
                    Name = "Salmon",
                    Description = "Cooked salmon",
                    Price = 10.99m,
                    ImageUrl = "https://images.unsplash.com/photo-1599084993091-1cb5c0721cc6?q=80&w=1770&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D",
                    QuantityInStock = 100,
                    CategoryId = seafoodProductCategory.Guid
                },
                new() {
                    Name = "Shrimp",
                    Description = "Cooked shrimp",
                    Price = 11.49m,
                    ImageUrl = "https://images.unsplash.com/photo-1559737558-2f5a35f4523b?q=80&w=1887&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D",
                    QuantityInStock = 200,
                    CategoryId = seafoodProductCategory.Guid
                },
                new() {
                    Name = "Crab",
                    Description = "Cooked crab",
                    Price = 12.49m,
                    ImageUrl = "https://plus.unsplash.com/premium_photo-1668143363099-1d9e04d4a3f7?q=80&w=1887&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D",
                    QuantityInStock = 69,
                    CategoryId = seafoodProductCategory.Guid
                },
                new() {
                    Name = "Lobster",
                    Description = "Cooked lobster",
                    Price = 13.49m,
                    ImageUrl = "https://images.unsplash.com/photo-1590759668628-05b0fc34bb70?q=80&w=1770&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D",
                    QuantityInStock = 96,
                    CategoryId = seafoodProductCategory.Guid
                },

                new() {
                    Name = "Oyster",
                    Description = "Fresh oyster",
                    Price = 14.49m,
                    ImageUrl = "https://images.unsplash.com/photo-1632027543405-925ae0b0f98c?q=80&w=1780&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D",
                    QuantityInStock = 96,
                    CategoryId = seafoodProductCategory.Guid
                }
            };

            await context.Products.AddRangeAsync(listFruitProduct);
            await context.SaveChangesAsync();
        }

        if (!context.Coupons.AsNoTracking().Any())
        {
            var coupons = new List<Coupon>
            {
                new() {
                    Name = "Black Friday",
                    Discount = 20,
                    MinimumAmount = 50,
                    CouponType = ECouponType.Percent,
                    ExpiryDate = DateTime.Now.AddDays(5)
                },
                new() {
                    Name = "Cyber Monday",
                    Discount = 10,
                    MinimumAmount = 100,
                    CouponType = ECouponType.Percent,
                    ExpiryDate = DateTime.Now.AddDays(10)
                },
                new() {
                    Name = "Christmas",
                    Discount = 50,
                    MinimumAmount = 200,
                    CouponType = ECouponType.Cash,
                    ExpiryDate = DateTime.Now.AddDays(30)
                }
            };

            await context.Coupons.AddRangeAsync(coupons);
            await context.SaveChangesAsync();
        }


        var firstUser = await context.Users.FirstAsync();
        var secondUser = await context.Users.Skip(1).FirstAsync();
        var firstProduct = await context.Products.FirstAsync();
        var secondProduct = await context.Products.Skip(1).FirstAsync();
        var thirdProduct = await context.Products.Skip(2).FirstAsync();
        var fourthProduct = await context.Products.Skip(3).FirstAsync();
        var fifthProduct = await context.Products.Skip(4).FirstAsync();

        if (!context.Reviews.AsNoTracking().Any())
        {
            var reviews = new List<Review>
            {
                new() {
                    ProductId = firstProduct.Guid,
                    UserId = secondUser.Id,
                    Rating = 5,
                    Content = "Great product"
                },
                new() {
                    ProductId = secondProduct.Guid,
                    UserId = secondUser.Id,
                    Rating = 4,
                    Content = "Good product"
                },
                new() {
                    ProductId = thirdProduct.Guid,
                    UserId = secondUser.Id,
                    Rating = 3,
                    Content = "Normal product"
                },
                new() {
                    ProductId = fourthProduct.Guid,
                    UserId = secondUser.Id,
                    Rating = 2,
                    Content = "Bad product"
                },
                new() {
                    ProductId = fifthProduct.Guid,
                    UserId = secondUser.Id,
                    Rating = 1,
                    Content = "Very bad product"
                }
            };

            await context.Reviews.AddRangeAsync(reviews);
            await context.SaveChangesAsync();
        }
        if (!context.Comments.AsNoTracking().Any())
        {
            var comments = new List<Comment>
            {
                new() {

                    UserId = firstUser.Id,
                    Content = "NOICE Great product",
                    ReviewId = null
                },
                new() {

                    UserId = firstUser.Id,
                    Content = "RIGHT Good product",
                    ReviewId = null
                },
                new() {

                    UserId = firstUser.Id,
                    Content = "Right Normal product",
                    ReviewId = null
                },
                new() {

                    UserId = firstUser.Id,
                    Content = "Please it's not Bad product 😥",
                    ReviewId = context.Reviews.First(x=>x.Rating == 2).Guid
                },
                new() {

                    UserId = firstUser.Id,
                    Content = "Oh no, please! I have children home😭",
                    ReviewId = context.Reviews.First(x=>x.Rating == 1).Guid
                }
            };

            await context.Comments.AddRangeAsync(comments);
            await context.SaveChangesAsync();
        }

        if (!context.Orders.AsNoTracking().Any())
        {
            var orders = new List<Order>
            {
                new() {
                    UserId = secondUser.Id,
                    ProductId = firstProduct.Guid,
                    Quantity = 1,
                    TotalPrice = firstProduct.Price,
                    Status = EOrderStatus.Pending,
                    OrderDate = DateTime.Now
                },
                new() {
                    UserId = secondUser.Id,
                    ProductId = secondProduct.Guid,
                    Quantity = 2,
                    TotalPrice = secondProduct.Price * 2,
                    Status = EOrderStatus.Processing,
                    OrderDate = DateTime.Now
                },
                new() {
                    UserId = secondUser.Id,
                    ProductId = thirdProduct.Guid,
                    Quantity = 3,
                    TotalPrice = thirdProduct.Price * 3,
                    Status = EOrderStatus.Shipped,
                    OrderDate = DateTime.Now
                },
                new() {
                    UserId = secondUser.Id,
                    ProductId = fourthProduct.Guid,
                    Quantity = 4,
                    TotalPrice = fourthProduct.Price * 4,
                    Status = EOrderStatus.Delivered,
                    OrderDate = DateTime.Now
                },
                new() {
                    UserId = secondUser.Id,
                    ProductId = fifthProduct.Guid,
                    Quantity = 5,
                    TotalPrice = fifthProduct.Price * 5,
                    Status = EOrderStatus.Cancelled,
                    OrderDate = DateTime.Now
                }
            };

            await context.Orders.AddRangeAsync(orders);
            await context.SaveChangesAsync();
        }
    }

}