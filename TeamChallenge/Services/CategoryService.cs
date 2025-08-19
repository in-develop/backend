using TeamChallenge.Models.Entities;
using TeamChallenge.Interfaces;

namespace TeamChallenge.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }


        public async Task<IEnumerable<CategoryEntity>> GetAllCategoriesAsync()
        {
                return await _categoryRepository.GetAllAsync(); 
        }

        public async Task<CategoryEntity?> GetCategoryByIdAsync(int id)
        {
            return await _categoryRepository.GetByIdAsync(id);
        }

        public async Task AddCategoryAsync(CategoryEntity category)
        {
            await _categoryRepository.AddAsync(category);
        }

        public async Task<bool> UpdateCategoryAsync(int id, CategoryEntity category)
        {
                if (await _categoryRepository.UpdateAsync(id, category))
                {
                    await _categoryRepository.SaveChangesAsync();
                    return true;
                }
                return false;
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var deleted = await _categoryRepository.DeleteAsync(id);
            if (!deleted)
                throw new KeyNotFoundException($"Category with Id={id} not found");
            return true;
        }





        //public async Task<List<CategoryReadDto>> GetAllAsync()
        //{
        //    return await _context.Categories
        //        .AsNoTracking()
        //        .Select(c => new CategoryReadDto
        //        {
        //            Id = c.Id,
        //            Name = c.Name,
        //        }).ToListAsync();
        //}

        //public async Task<CategoryReadDto?> GetByIdAsync(int id)
        //{
        //    return await _context.Categories
        //        .AsNoTracking()
        //        .Where(c => c.Id == id)
        //        .Select(c => new CategoryReadDto
        //        {
        //            Id = c.Id,
        //            Name = c.Name,
        //        }).FirstOrDefaultAsync();
        //}

        //// Id Error
        //public async Task<CategoryReadDto> CreateAsync(CategoryCreateDto dto)
        //{
        //    var category = new CategoryEntity
        //    { 
        //        Name = dto.Name 
        //    };
        //    _context.Categories.Add(category);
        //    await _context.SaveChangesAsync();

        //    return new CategoryReadDto
        //    {
        //        Name = category.Name,
        //    };
        //}

        //public async Task<bool> UpdateAsync(int id, CategoryAddDto dto)
        //{
        //    var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
        //    if (category == null)
        //    {
        //        return false;
        //    }
        //    category.Name = dto.Name;
        //    await _context.SaveChangesAsync();
        //    return true;
        //}

        //public async Task<bool> DeleteAsync(int id)
        //{
        //    var category = await _context.Categories.FindAsync(id);
        //    if (category == null)
        //    {
        //        return false;
        //    }

        //    _context.Categories.Remove(category);
        //    await _context.SaveChangesAsync();
        //    return true;
        //}
    }
}
