using TeamChallenge.Models.Entities;
using TeamChallenge.Models.DTOs.Category;
using TeamChallenge.Repositories;
using TeamChallenge.Logic;

namespace TeamChallenge.Logic
{
    public class CategoryService : ICategoryLogic
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryService(RepositoryFactory factory)
        {
            _categoryRepository = (ICategoryRepository)factory.GetRepository<CategoryEntity>();
        }


        public async Task<IEnumerable<CategoryEntity>> GetAllCategoriesAsync()
        {
            return await _categoryRepository.GetAllAsync();
        }

        public async Task<CategoryEntity?> GetCategoryByIdAsync(int id)
        {
            return await _categoryRepository.GetByIdAsync(id);
        }

        public async Task<CategoryEntity> CreateCategoryAsync(CategoryCreateDto dto)
        {
            var category = new CategoryEntity { Name = dto.Name };
            //await _categoryRepository.CreateAsync(category);
            return category;
        }

        public async Task<bool> UpdateCategoryAsync(int id, CategoryUpdateDto dto)
        {
            var category = new CategoryEntity { Id = id, Name = dto.Name };
            /*if (await _categoryRepository.UpdateAsync(id, category))
            {
                return true;
            }*/
            return false;
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var deleted = await _categoryRepository.DeleteAsync(id);
            if (!deleted)
                throw new KeyNotFoundException($"Category with Id={id} not found");
            return true;
        }
    }
}
