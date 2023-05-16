namespace MusicShop.Front.Services.Contracts;
public interface ICategoryService
{
    Task<IEnumerable<CategoryViewModel>> GetAllCategories(string token);
}
