using TeamChallenge.Models.DTOs.Category;

namespace TeamChallenge.Interfaces
{
    public interface ICategory
    {
        Task<List<CategoryReadDto>> GetAllAsync();
        Task<CategoryReadDto?> GetByIdAsync(int id);
        Task<CategoryReadDto> CreateAsync(CategoryCreateDto dto);
        Task<bool> UpdateAsync(int id, CategoryAddDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
