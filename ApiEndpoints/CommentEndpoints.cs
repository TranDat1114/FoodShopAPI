

using Microsoft.AspNetCore.Authorization;

namespace FoodShopAPI;

public static class CommentEndpoints
{
    public static void MapCommentEndpoints(this WebApplication app)
    {
        var commentGroup = app.MapGroup("comments").WithTags("Comments");
        commentGroup.MapGet("", async (int page, int pageSize, IUnitOfWork unitOfWork) =>
        {
            var comments = await unitOfWork.GetRepository<Comment>().PaginateAsync(page, pageSize, x => x.CreatedAt);
            return Results.Ok(comments);
        });

        commentGroup.MapGet("{guid}", async (string guid, IUnitOfWork unitOfWork) =>
        {
            var comment = await unitOfWork.GetRepository<Comment>().GetByGuidAsync(guid);
            if (comment == null)
            {
                return Results.NotFound();
            }
            return Results.Ok(comment);
        });

        commentGroup.MapPost("", [Authorize] async (Comment comment, IUnitOfWork unitOfWork) =>
        {
            await unitOfWork.GetRepository<Comment>().AddAsync(comment);

            return Results.Created($"/comments/{comment.Guid}", comment);
        });

        commentGroup.MapPut("{guid}", [Authorize] async (string guid, Comment comment, IUnitOfWork unitOfWork) =>
        {
            var existingComment = await unitOfWork.GetRepository<Comment>().GetByGuidAsync(guid);
            if (existingComment == null)
            {
                return Results.NotFound();
            }
            comment.Guid = existingComment.Guid;
            unitOfWork.GetRepository<Comment>().Update(comment);
            return Results.NoContent();
        });

        commentGroup.MapDelete("{guid}", [Authorize] async (string guid, IUnitOfWork unitOfWork) =>
        {
            var existingComment = await unitOfWork.GetRepository<Comment>().GetByGuidAsync(guid);
            if (existingComment == null)
            {
                return Results.NotFound();
            }
            await unitOfWork.GetRepository<Comment>().DeleteAsync(guid);
            return Results.NoContent();
        });

    }

}
