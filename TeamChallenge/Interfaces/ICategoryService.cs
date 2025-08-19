using TeamChallenge.Models.DTOs.Category;
using TeamChallenge.Models.Entities;

namespace TeamChallenge.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryEntity>> GetAllCategoriesAsync();
        Task<CategoryEntity?> GetCategoryByIdAsync(int id);
        Task AddCategoryAsync(CategoryEntity category);
        Task<bool> UpdateCategoryAsync(int id, CategoryEntity category);
        Task<bool> DeleteCategoryAsync(int id);
    }
}
