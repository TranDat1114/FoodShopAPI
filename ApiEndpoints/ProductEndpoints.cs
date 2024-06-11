using Microsoft.AspNetCore.Authorization;

namespace FoodShopAPI;

public static class ProductEndpoints
{
    public static void MapProductEndpoints(this WebApplication app)
    {
        var productGroup = app.MapGroup("products").WithTags("Products");
        productGroup.MapGet("", async (int page, int pageSize, IUnitOfWork unitOfWork) =>
        {
            var products = await unitOfWork.GetRepository<Product>().PaginateAsync(page, pageSize, x => x.CreatedAt);
            return Results.Ok(products);
        });

        productGroup.MapGet("{guid}", async (string guid, IUnitOfWork unitOfWork) =>
        {
            var product = await unitOfWork.GetRepository<Product>().GetByGuidAsync(guid);
            if (product == null)
            {
                return Results.NotFound();
            }
            return Results.Ok(product);
        });

        productGroup.MapPost("",[Authorize(Policy = SD.Admin)] async (Product product, IUnitOfWork unitOfWork) =>
        {
            await unitOfWork.GetRepository<Product>().AddAsync(product);

            return Results.Created($"/products/{product.Guid}", product);
        });

        productGroup.MapPut("{guid}",[Authorize(Policy = SD.Admin)] async (string guid, Product product, IUnitOfWork unitOfWork) =>
        {
            var existingProduct = await unitOfWork.GetRepository<Product>().GetByGuidAsync(guid);
            if (existingProduct == null)
            {
                return Results.NotFound();
            }
            product.Guid = existingProduct.Guid;
            unitOfWork.GetRepository<Product>().Update(product);
            return Results.NoContent();
        });

        productGroup.MapDelete("{guid}",[Authorize(Policy = SD.Admin)] async (string guid, IUnitOfWork unitOfWork) =>
        {
            var existingProduct = await unitOfWork.GetRepository<Product>().GetByGuidAsync(guid);
            if (existingProduct == null)
            {
                return Results.NotFound();
            }
            await unitOfWork.GetRepository<Product>().DeleteAsync(guid);
            return Results.NoContent();
        });

    }

}
