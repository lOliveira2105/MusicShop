using MusicShop.Front.Models;

namespace MusicShop.Front.Controllers;

public class CartController : Controller
{
        private readonly ICartServices _cartServices;

    public CartController(ICartServices cartServices)
    {
        _cartServices = cartServices;
    }
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Index()
    {
        CartViewModel? cartVM = await GetCartByUser();

        if (cartVM is null)
        {
            ModelState.AddModelError("CartNotFound", "Does not exist a cart yet...Come on Shopping...");
            return View("/Views/Cart/CartNotFound.cshtml");
        }

        return View(cartVM);

    }
    private async Task<CartViewModel?> GetCartByUser()
    {

        var cart = await _cartServices.GetCartByUserIdAsync(GetUserId(), await GetAccessToken());

        if (cart?.CartHeader is not null)
        {
            //if (!string.IsNullOrEmpty(cart.CartHeader.CouponCode))
            //{
            //    var coupon = await _couponServices.GetDiscountCoupon(cart.CartHeader.CouponCode,
            //                                                        await GetAccessToken());
            //    if (coupon?.CouponCode is not null)
            //    {
            //        cart.CartHeader.Discount = coupon.Discount;
            //    }
            //}
            foreach (var item in cart.CartItems)
            {
                cart.CartHeader.TotalAmount += (item.Product.Price * item.Quantity);
            }

            //cart.CartHeader.TotalAmount = cart.CartHeader.TotalAmount -
            //                             (cart.CartHeader.TotalAmount *
            //                              cart.CartHeader.Discount) / 100;
        }
        return cart;
    }
    public async Task<IActionResult> RemoveItem(int id)
    {
        var result = await _cartServices.RemoveFromCartAsync(id, await GetAccessToken());

        if (result)
        {
            return RedirectToAction(nameof(Index));
        }
        return View(id);
    }

    private async Task<string> GetAccessToken()
    {
        return await HttpContext.GetTokenAsync("access_token");
    }

    private string GetUserId()
    {
        return User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value;
    }
}
