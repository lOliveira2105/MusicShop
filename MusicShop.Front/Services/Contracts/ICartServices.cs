using MusicShop.Front.Models;

namespace MusicShop.Front.Services.Contracts;

public interface ICartServices
{
    Task<CartViewModel> GetCartByUserIdAsync(string userId, string token);
    Task<CartViewModel> AddItemToCartAsync(CartViewModel cartVM, string token);
    Task<CartViewModel> UpdateCartAsync(CartViewModel cartVM, string token);
    Task<bool> RemoveFromCartAsync(int cartId, string token);

    //Implementação futura
    Task<bool> ApplyCouponAsync(CartViewModel cartVM, string couponCode, string token);
    Task<bool> RemoveCouponAsync(string userId, string token);
    Task<bool> ClearCartAsync(string userId, string token);

    Task<CartViewModel> CheckoutAsync(CartHeaderViewModel cartHeader, string token);

}
