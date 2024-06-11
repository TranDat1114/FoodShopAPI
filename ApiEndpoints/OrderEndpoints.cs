using Microsoft.AspNetCore.Authorization;

namespace FoodShopAPI;

public static class OrderEndpoints
{
    public static void MapOrderEndpoints(this WebApplication app)
    {
        var orderGroup = app.MapGroup("orders").WithTags("Orders");
        orderGroup.MapGet("", async (int page, int pageSize, IUnitOfWork unitOfWork) =>
        {
            var orders = await unitOfWork.GetRepository<Order>().PaginateAsync(page, pageSize, x => x.CreatedAt);
            return Results.Ok(orders);
        });

        orderGroup.MapGet("{guid}", async (string guid, IUnitOfWork unitOfWork) =>
        {
            var order = await unitOfWork.GetRepository<Order>().GetByGuidAsync(guid);
            if (order == null)
            {
                return Results.NotFound();
            }
            return Results.Ok(order);
        });

        orderGroup.MapPost("", [Authorize] async (Order order, IUnitOfWork unitOfWork) =>
        {
            await unitOfWork.GetRepository<Order>().AddAsync(order);
            return Results.Created($"/orders/{order.Guid}", order);
        });

        orderGroup.MapPut("{guid}", [Authorize(Policy = SD.Admin)] async (string guid, Order order, IUnitOfWork unitOfWork) =>
        {
            var existingOrder = await unitOfWork.GetRepository<Order>().GetByGuidAsync(guid);
            if (existingOrder == null)
            {
                return Results.NotFound();
            }
            order.Guid = existingOrder.Guid;
            unitOfWork.GetRepository<Order>().Update(order);
            return Results.NoContent();
        });

        orderGroup.MapDelete("{guid}", [Authorize(Policy = SD.Admin)] async (string guid, IUnitOfWork unitOfWork) =>
        {
            var existingOrder = await unitOfWork.GetRepository<Order>().GetByGuidAsync(guid);
            if (existingOrder == null)
            {
                return Results.NotFound();
            }
            await unitOfWork.GetRepository<Order>().DeleteAsync(guid);
            return Results.NoContent();
        });
    }
}
