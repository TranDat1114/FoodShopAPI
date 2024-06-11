using Microsoft.AspNetCore.Authorization;

namespace FoodShopAPI;

public static class ImageEndpoints
{
    public static void MapImageEndpoints(this WebApplication app)
    {
        #region Crud
        var imageGroup = app.MapGroup("images").WithTags("Images");
        imageGroup.MapGet("", async (int page, int pageSize, IUnitOfWork unitOfWork) =>
        {
            var images = await unitOfWork.GetRepository<Image>().PaginateAsync(page, pageSize, x => x.CreatedAt);
            return Results.Ok(images);
        });

        imageGroup.MapGet("{guid}", async (string guid, IUnitOfWork unitOfWork) =>
        {
            var image = await unitOfWork.GetRepository<Image>().GetByGuidAsync(guid);
            if (image == null)
            {
                return Results.NotFound();
            }
            return Results.Ok(image);
        });

        imageGroup.MapPost("", [Authorize(Policy = SD.Admin)] async (Image image, IUnitOfWork unitOfWork) =>
        {
            await unitOfWork.GetRepository<Image>().AddAsync(image);

            return Results.Created($"/images/{image.Guid}", image);
        });

        imageGroup.MapPut("{guid}",[Authorize(Policy = SD.Admin)] async (string guid, Image image, IUnitOfWork unitOfWork) =>
        {
            var existingImage = await unitOfWork.GetRepository<Image>().GetByGuidAsync(guid);
            if (existingImage == null)
            {
                return Results.NotFound();
            }
            image.Guid = existingImage.Guid;
            unitOfWork.GetRepository<Image>().Update(image);
            return Results.NoContent();
        });

        imageGroup.MapDelete("{guid}", [Authorize(Policy = SD.Admin)] async (string guid, IUnitOfWork unitOfWork) =>
        {
            var existingImage = await unitOfWork.GetRepository<Image>().GetByGuidAsync(guid);
            if (existingImage == null)
            {
                return Results.NotFound();
            }
            await unitOfWork.GetRepository<Image>().DeleteAsync(guid);
            return Results.NoContent();
        });
        #endregion

        imageGroup.MapGet("group/{groupType}", async (string groupType, int page, int pageSize, IUnitOfWork unitOfWork) =>
        {
            var images = await unitOfWork.GetRepository<Image>().GetAllAsync(x => x.GroupType == groupType, page, pageSize, x => x.CreatedAt);
            return Results.Ok(images);
        }).WithDisplayName("GetImagesByGroupTypePaginated");
    }

}
