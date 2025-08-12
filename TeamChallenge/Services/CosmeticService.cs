using Microsoft.EntityFrameworkCore;
using TeamChallenge.Models.Models;
using TeamChallenge.Models.DTOs;
using TeamChallenge.DbContext;
using TeamChallenge.Interfaces;

namespace TeamChallenge.Services
{
    public class CosmeticService: ICosmetic
    {
        private readonly CosmeticStoreDbContext _context;

        public CosmeticService(CosmeticStoreDbContext context)
        {
            _context = context;
        }

        public async Task<List<CosmeticReadDto>> GetAllAsync()
        {
            return await _context.Cosmetiс
                .Select(c => new CosmeticReadDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    Price = c.Price,
                    Category = c.Category
                }).ToListAsync();
        }
        public async Task<CosmeticReadDto?> GetByIdAsync(int id)
        {
            return await _context.Cosmetiс
                .Select(c => new CosmeticReadDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    Price = c.Price,
                    Category = c.Category
                }).FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<CosmeticReadDto> CreateAsync(CosmeticCreateDto dto)
        {
            var cosmetic = new Cosmetic
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                Category = await _context.Category
                .Where(cat => dto.CategoryIds.Contains(cat.Id))
                .ToListAsync()
            };

            _context.Cosmetiс.Add(cosmetic);
            await _context.SaveChangesAsync();

            return new CosmeticReadDto
            {
                Id = cosmetic.Id,
                Name = cosmetic.Name,
                Description = cosmetic.Description,
                Price = cosmetic.Price,
                Category = cosmetic.Category
            };
        }

        public async Task<bool> UpdateAsync(int id, CosmeticCreateDto dto)
        {
            var cosmetic = await _context.Cosmetiс
                .FirstOrDefaultAsync(c => c.Id == id);

            if (cosmetic == null) 
            {
                return false;
            }

            cosmetic.Name = dto.Name;
            cosmetic.Description = dto.Description;
            cosmetic.Price = dto.Price;
            cosmetic.Category = await _context.Category
                .Where(cat => dto.CategoryIds.Contains(cat.Id))
                .ToListAsync();

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var cosmetic = await _context.Cosmetiс.FindAsync(id);
            if (cosmetic == null) 
            {
                return false;
            }

            _context.Cosmetiс.Remove(cosmetic);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
