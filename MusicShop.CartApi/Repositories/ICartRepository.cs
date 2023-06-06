

namespace MusicShop.CartApi.Repositories;

public interface ICartRepository
{
    Task<CartDTO> GetCartByUserIdAsync(string userId);
    Task<CartDTO> UpdateCartAsync(CartDTO cart);
    Task<bool> CleanCartAsync (string userId);
    Task<bool> DeleteItemCartAsync(int cartItemId);

    Task<bool> ApplyCouponsAsyn(String userID, string couponCode);
    Task<bool> DeleteCouponAsyn(string userId);
        

}
