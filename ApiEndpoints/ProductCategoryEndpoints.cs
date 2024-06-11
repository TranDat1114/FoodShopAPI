using Microsoft.AspNetCore.Authorization;

namespace FoodShopAPI;

public static class ProductCategoryEndpoints
{
    public static void MapProductCategoryEndpoints(this WebApplication app)
    {
        var productCategoryGroup = app.MapGroup("productcategories").WithTags("ProductCategories");
        productCategoryGroup.MapGet("", async (int page, int pageSize, IUnitOfWork unitOfWork) =>
        {
            var productCategories = await unitOfWork.GetRepository<ProductCategory>().PaginateAsync(page, pageSize, x => x.CreatedAt);
            return Results.Ok(productCategories);
        });

        productCategoryGroup.MapGet("{guid}", async (string guid, IUnitOfWork unitOfWork) =>
        {
            var productCategory = await unitOfWork.GetRepository<ProductCategory>().GetByGuidAsync(guid);
            if (productCategory == null)
            {
                return Results.NotFound();
            }
            return Results.Ok(productCategory);
        });

        productCategoryGroup.MapPost("", [Authorize(Policy = SD.Admin)] async (ProductCategory productCategory, IUnitOfWork unitOfWork) =>
        {
            await unitOfWork.GetRepository<ProductCategory>().AddAsync(productCategory);

            return Results.Created($"/productcategories/{productCategory.Guid}", productCategory);
        });

        productCategoryGroup.MapPut("{guid}", [Authorize(Policy = SD.Admin)] async (string guid, ProductCategory productCategory, IUnitOfWork unitOfWork) =>
        {
            var existingProductCategory = await unitOfWork.GetRepository<ProductCategory>().GetByGuidAsync(guid);
            if (existingProductCategory == null)
            {
                return Results.NotFound();
            }
            productCategory.Guid = existingProductCategory.Guid;
            unitOfWork.GetRepository<ProductCategory>().Update(productCategory);
            return Results.NoContent();
        });

        productCategoryGroup.MapDelete("{guid}", [Authorize(Policy = SD.Admin)] async (string guid, IUnitOfWork unitOfWork) =>
        {
            var existingProductCategory = await unitOfWork.GetRepository<ProductCategory>().GetByGuidAsync(guid);
            if (existingProductCategory == null)
            {
                return Results.NotFound();
            }
            await unitOfWork.GetRepository<ProductCategory>().DeleteAsync(guid);
            return Results.NoContent();
        });
    }

}
