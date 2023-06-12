namespace MusicShop.Front.Services;

public class CartService : ICartServices
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly JsonSerializerOptions? _options;
    private const string apiEndpoint = "/api/cart*";
    private CartViewModel cartVM = new CartViewModel();

    public CartService(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
        _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

    }
    public async Task<CartViewModel> GetCartByUserIdAsync(string userId, string token)
    {
        var client = _clientFactory.CreateClient("CartApi");
        PutTokenInHeaderAuthorization(token, client);

        using (var response = await client.GetAsync($"{apiEndpoint}/getcart/{userId}"))
        {
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStreamAsync();
                cartVM = await JsonSerializer
                              .DeserializeAsync<CartViewModel>
                              (apiResponse, _options);
            }
            else
            {
                return null;
            }
        }
        return cartVM;
    }


    public Task<CartViewModel> AddItemToCartAsync(CartViewModel cartVM, string token)
    {
        throw new NotImplementedException();
    }

    public Task<CartViewModel> UpdateCartAsync(CartViewModel cartVM, string token)
    {
        throw new NotImplementedException();
    }

    public Task<bool> RemoveFromCartAsync(int cartId, string token)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ApplyCouponAsync(CartViewModel cartVM, string couponCode, string token)
    {
        throw new NotImplementedException();
    }

    public Task<bool> RemoveCouponAsync(string userId, string token)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ClearCartAsync(string userId, string token)
    {
        throw new NotImplementedException();
    }

    public Task<CartViewModel> CheckoutAsync(CartHeaderViewModel cartHeader, string token)
    {
        throw new NotImplementedException();
    }



    private void PutTokenInHeaderAuthorization(string token, HttpClient client)
    {
        throw new NotImplementedException();
    }
}
