﻿
using System.Globalization;

namespace MusicShop.Front.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IProductService _productService;
    private readonly ICartServices _cartService;

    public HomeController(ILogger<HomeController> logger, IProductService productService, ICartServices cartServices)
    {
        _logger = logger;
        _productService = productService;
        _cartService = cartServices;
    }

    public async Task<IActionResult> Index()
    {
        var result = await _productService.GetAllProducts(string.Empty);

        if (result is null)
            return View("Error");

        return View(result);
    }
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<ProductViewModel>> ProductDetails(int id)
    {
        var token = await HttpContext.GetTokenAsync("access_token");
        var product = await _productService.FindProductById(id, token);

        if (product is null)
            return View("Error");

        return View(product);
    }
    [HttpPost]
    [ActionName("ProductDetails")]
    [Authorize]
    public async Task<ActionResult<ProductViewModel>> ProductDetailsPost(ProductViewModel ProductVM)
    {
        var token = await HttpContext.GetTokenAsync("access_token");

        CartViewModel cart = new()
        {
            CartHeader = new CartHeaderViewModel
            {
                UserId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value
            }
        };
        CartItemViewModel cartItem = new()
        {
            Quantity = ProductVM.Quantity,
            ProductId = ProductVM.Id,
            Product = await _productService.FindProductById(ProductVM.Id, token)
        };
        List<CartItemViewModel> cartItemsVm = new List<CartItemViewModel>();
        cartItemsVm.Add(cartItem);
        cart.CartItems = cartItemsVm;

        var result = await _cartService.AddItemToCartAsync(cart, token);
        if (result is not null)
        {
            return RedirectToAction(nameof(Index));
        }
        return View(ProductVM);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    [Authorize]
    public async Task<IActionResult> Login()
    {
        var accessToken = await HttpContext.GetTokenAsync("access_token");
        return RedirectToAction(nameof(Index));
    }
    public IActionResult Logout()
    {
        return SignOut("Cookies", "oidc");
    }

}