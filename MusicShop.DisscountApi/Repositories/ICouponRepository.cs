namespace MusicShop.DiscountApi.Repositories;

public interface ICouponRepository
{
    Task<CouponDTO> GetCouponByCode(string Couponcode);
}
