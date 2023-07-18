using Inveasy.Models;

namespace Inveasy.Services.ProjectServices
{
    public interface ICategoryService
    {
        string StatusMessage { get; }

        Task<Category> GetCategoryAsync(int categoryId);
        Task<Category> GetCategoryAsync(string categoryName);
        Task<List<Category>> GetCategoriesAsync();
    }
}
