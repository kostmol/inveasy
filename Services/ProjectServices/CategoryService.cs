using Inveasy.Data;
using Inveasy.Models;
using Microsoft.EntityFrameworkCore;
using static Inveasy.Services.ServiceStatus;

namespace Inveasy.Services.ProjectServices
{
    public class CategoryService : ICategoryService
    {
        private readonly InveasyContext _context;
        private readonly CategoryStatus _status;

        public string StatusMessage { get; private set; } = "CategoryService initialized";

        public CategoryService(InveasyContext context, CategoryStatus serviceStatus)
        {
            _context = context;
            _status = serviceStatus;
        }

        #region GET services
        // Method that return role from given role id
        public async Task<Category> GetCategoryAsync(int categoryId)
        {
            var categories = await GetCategoriesAsync();
            return categories?.FirstOrDefault(d => d.Id == categoryId);
        }

        public async Task<Category> GetCategoryAsync(string categoryName)
        {
            var categories = await GetCategoriesAsync();
            return categories?.FirstOrDefault(d => d.Name == categoryName);
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            try
            {
                return await _context.Category.ToListAsync();
            }
            catch (Exception ex)
            {
                StatusMessage = _status.ExceptionGetStatus(ex.Message);
                return null;
            }
        }
        #endregion
    }
}
