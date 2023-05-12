
using System.Text;

namespace MusicShop.Front.Services;
public class ProductService : IProductService
{
    private readonly IHttpClientFactory _clientFactory;
    private const string apiEndpoint = "/api/products/";
    private readonly JsonSerializerOptions _options;
    private ProductViewModel productView;
    private IEnumerable<ProductViewModel> products;

    public ProductService(IHttpClientFactory ClientFactory, ICategoryService categoryService)
    {
        _clientFactory = ClientFactory;
        _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    public async Task<IEnumerable<ProductViewModel>> GetAllProducts()
    {
        var client = _clientFactory.CreateClient("ProductApi");

        using (var response = await client.GetAsync(apiEndpoint))
        {
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStreamAsync();
                products = await JsonSerializer
                            .DeserializeAsync<IEnumerable<ProductViewModel>>(apiResponse, _options);
            }
            else
            {
                return null;
            }
        }
        return products;
    }
    public async Task<ProductViewModel> FindProductById(int id)
    {
        var client = _clientFactory.CreateClient("ProductApi");

        using (var response = await client.GetAsync(apiEndpoint + id))
        {
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStreamAsync();
                productView = await JsonSerializer
                          .DeserializeAsync<ProductViewModel>(apiResponse, _options);
            }
            else
            {
                return null;
            }
        }
        return productView;


    }
    public async Task<ProductViewModel> CreateProduct(ProductViewModel productVM)
    {
        var client = _clientFactory.CreateClient("ProductApi");

        StringContent content = new StringContent(JsonSerializer.Serialize(productVM),
                                Encoding.UTF8, "application/json");

        using (var response = await client.PostAsync(apiEndpoint, content))
        {
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStreamAsync();
                productVM = await JsonSerializer
                            .DeserializeAsync<ProductViewModel>(apiResponse, _options);
            }
            else
            {
                return null;
            }
        }
        return productVM;
    }
    public async Task<ProductViewModel> UpdateProduct(ProductViewModel productVM)
    {
        var client = _clientFactory.CreateClient("ProductApi");
        ProductViewModel productUpdated = new ProductViewModel();

        using (var response = await client.PutAsJsonAsync(apiEndpoint, productVM))
        {
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStreamAsync();
                productUpdated = await JsonSerializer
                                 .DeserializeAsync<ProductViewModel>(apiResponse, _options);
            }
            else
            {
                return null;
            }
        }
        return productUpdated;

    }
    public async Task<bool> DeleteProductById(int id)
    {
        var client = _clientFactory.CreateClient("ProductApi");

        using (var response = await client.DeleteAsync(apiEndpoint + id))
        {
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
        }
        return false;
    }

}
