﻿namespace MusicShop.Front.Controllers;

public class ProductController : Controller
{
    private readonly IProductService _productService;
    private readonly ICategoryService _categoryService;
    public ProductController(IProductService productService, ICategoryService categoryService)
    {
        _productService = productService;
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductViewModel>>> Index()
    {
        
        var result = await _productService.GetAllProducts(await GetAcessToken());

        if (result is null)
            return View("Error");

        return View(result);
    }

  

    [HttpGet]
    public async Task<IActionResult> CreateProduct()
    {
        ViewBag.CategoryId = new SelectList(await
             _categoryService.GetAllCategories(await GetAcessToken()), "CategoryId", "Name");

        return View();
    }
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateProduct(ProductViewModel productVM)
    {
        if (ModelState.IsValid)
        {
            var result = await _productService.CreateProduct(productVM, await GetAcessToken());

            if (result != null)
                return RedirectToAction(nameof(Index));
        }
        else
        {
            ViewBag.CategoryId = new SelectList(await
                                 _categoryService.GetAllCategories(await GetAcessToken()), "CategoryId", "Name");
        }
        return View(productVM);
    }

    [HttpGet]
    public async Task<IActionResult> UpdateProduct(int id)
    {
        ViewBag.CategoryId = new SelectList(await
                           _categoryService.GetAllCategories(await GetAcessToken()), "CategoryId", "Name");

        var result = await _productService.FindProductById(id, await GetAcessToken());

        if (result is null)
            return View("Error");

        return View(result);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> UpdateProduct(ProductViewModel productVM)
    {
        if (ModelState.IsValid)
        {
            var result = await _productService.UpdateProduct(productVM, await GetAcessToken());

            if (result is not null)
                return RedirectToAction(nameof(Index));
        }
        return View(productVM);
    }
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<ProductViewModel>> DeleteProduct(int id)
    {
        var result = await _productService.FindProductById(id, await GetAcessToken());

        if (result is null)
            return View("Error");

        return View(result);
    }
    [Authorize(Roles = Role.Admin)]
    [HttpPost(), ActionName("DeleteProduct")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var result = await _productService.DeleteProductById(id, await GetAcessToken());

        if (!result)
            return View("Error");

        return RedirectToAction("Index");
    }
    private async Task<string> GetAcessToken()
    {
        return await HttpContext.GetTokenAsync("access_token");
    }

}
