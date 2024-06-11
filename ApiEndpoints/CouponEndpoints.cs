using Microsoft.AspNetCore.Authorization;

namespace FoodShopAPI;

public static class CouponEndpoints
{
    public static void MapCouponEndpoints(this WebApplication app)
    {
        var couponGroup = app.MapGroup("coupons").WithTags("Coupons");
        couponGroup.MapGet("", async (int page, int pageSize, IUnitOfWork unitOfWork) =>
        {
            var coupons = await unitOfWork.GetRepository<Coupon>().PaginateAsync(page, pageSize, x => x.CreatedAt);
            return Results.Ok(coupons);
        });

        couponGroup.MapGet("{guid}", async (string guid, IUnitOfWork unitOfWork) =>
        {
            var coupon = await unitOfWork.GetRepository<Coupon>().GetByGuidAsync(guid);
            if (coupon == null)
            {
                return Results.NotFound();
            }
            return Results.Ok(coupon);
        });

        couponGroup.MapPost("", [Authorize(Policy = SD.Admin)] async (Coupon coupon, IUnitOfWork unitOfWork) =>
        {
            await unitOfWork.GetRepository<Coupon>().AddAsync(coupon);

            return Results.Created($"/coupons/{coupon.Guid}", coupon);
        });

        couponGroup.MapPut("{guid}", [Authorize(Policy = SD.Admin)] async (string guid, Coupon coupon, IUnitOfWork unitOfWork) =>
        {
            var existingCoupon = await unitOfWork.GetRepository<Coupon>().GetByGuidAsync(guid);
            if (existingCoupon == null)
            {
                return Results.NotFound();
            }
            coupon.Guid = existingCoupon.Guid;
            unitOfWork.GetRepository<Coupon>().Update(coupon);
            return Results.NoContent();
        });

        couponGroup.MapDelete("{guid}", [Authorize(Policy = SD.Admin)] async (string guid, IUnitOfWork unitOfWork) =>
        {
            var existingCoupon = await unitOfWork.GetRepository<Coupon>().GetByGuidAsync(guid);
            if (existingCoupon == null)
            {
                return Results.NotFound();
            }
            await unitOfWork.GetRepository<Coupon>().DeleteAsync(guid);
            return Results.NoContent();
        });

    }

}
