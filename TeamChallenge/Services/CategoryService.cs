using Microsoft.EntityFrameworkCore;
using TeamChallenge.DbContext;
using TeamChallenge.Models.Entities;
using TeamChallenge.Interfaces;
using TeamChallenge.Models.DTOs.Category;

namespace TeamChallenge.Services
{
    public class CategoryService : ICategory
    {
        private readonly CosmeticStoreDbContext _context;

        public CategoryService(CosmeticStoreDbContext context)
        {
            _context = context;
        }

        public async Task<List<CategoryReadDto>> GetAllAsync()
        {
            return await _context.Category
                .AsNoTracking()
                .Select(c => new CategoryReadDto
                {
                    Id = c.Id,
                    Name = c.Name,
                }).ToListAsync();
        }

        public async Task<CategoryReadDto?> GetByIdAsync(int id)
        {
            return await _context.Category
                .AsNoTracking()
                .Where(c => c.Id == id)
                .Select(c => new CategoryReadDto
                {
                    Id = c.Id,
                    Name = c.Name,
                }).FirstOrDefaultAsync();
        }

        // Id Error
        public async Task<CategoryReadDto> CreateAsync(CategoryCreateDto dto)
        {
            var category = new Category
            { 
                Name = dto.Name 
            };
            _context.Category.Add(category);
            await _context.SaveChangesAsync();

            return new CategoryReadDto
            {
                Name = category.Name,
            };
        }

        public async Task<bool> UpdateAsync(int id, CategoryAddDto dto)
        {
            var category = await _context.Category.FirstOrDefaultAsync(c => c.Id == id);
            if (category == null)
            {
                return false;
            }
            category.Name = dto.Name;
            await _context.SaveChangesAsync(); 
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var category = await _context.Category.FindAsync(id);
            if (category == null) 
            {
                return false;
            }

            _context.Category.Remove(category);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
