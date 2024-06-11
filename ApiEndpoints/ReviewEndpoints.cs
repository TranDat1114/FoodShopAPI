using Microsoft.AspNetCore.Authorization;

namespace FoodShopAPI;

public static class ReviewEndpoints
{
    public static void MapReviewEndpoints(this WebApplication app)
    {
        var reviewGroup = app.MapGroup("reviews").WithTags("Reviews");
        reviewGroup.MapGet("", async (int page, int pageSize, IUnitOfWork unitOfWork) =>
        {
            var reviews = await unitOfWork.GetRepository<Review>().PaginateAsync(page, pageSize, x => x.CreatedAt);
            return Results.Ok(reviews);
        });

        reviewGroup.MapGet("{guid}", async (string guid, IUnitOfWork unitOfWork) =>
        {
            var review = await unitOfWork.GetRepository<Review>().GetByGuidAsync(guid);
            if (review == null)
            {
                return Results.NotFound();
            }
            return Results.Ok(review);
        });

        reviewGroup.MapPost("",[Authorize] async (Review review, IUnitOfWork unitOfWork) =>
        {
            await unitOfWork.GetRepository<Review>().AddAsync(review);

            return Results.Created($"/reviews/{review.Guid}", review);
        });

        reviewGroup.MapPut("{guid}",[Authorize] async (string guid, Review review, IUnitOfWork unitOfWork) =>
        {
            var existingReview = await unitOfWork.GetRepository<Review>().GetByGuidAsync(guid);
            if (existingReview == null)
            {
                return Results.NotFound();
            }
            review.Guid = existingReview.Guid;
            unitOfWork.GetRepository<Review>().Update(review);
            return Results.NoContent();
        });

        reviewGroup.MapDelete("{guid}", [Authorize] async (string guid, IUnitOfWork unitOfWork) =>
        {
            var existingReview = await unitOfWork.GetRepository<Review>().GetByGuidAsync(guid);
            if (existingReview == null)
            {
                return Results.NotFound();
            }
            await unitOfWork.GetRepository<Review>().DeleteAsync(guid);
            return Results.NoContent();
        });
    }

}
