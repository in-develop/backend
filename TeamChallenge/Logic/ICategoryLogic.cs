using TeamChallenge.Models.DTOs.Category;
using TeamChallenge.Models.Entities;

namespace TeamChallenge.Logic
{
    public interface ICategoryLogic
    {
        Task<IEnumerable<CategoryEntity>> GetAllCategoriesAsync();
        Task<CategoryEntity?> GetCategoryByIdAsync(int id);
        Task<CategoryEntity> CreateCategoryAsync(CategoryCreateDto dto);
        Task<bool> UpdateCategoryAsync(int id, CategoryUpdateDto dto);
        Task<bool> DeleteCategoryAsync(int id);
    }
}
