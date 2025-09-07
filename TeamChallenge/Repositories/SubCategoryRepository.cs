using Microsoft.EntityFrameworkCore;
using TeamChallenge.DbContext;
using TeamChallenge.Models.Entities;

namespace TeamChallenge.Repositories
{
    public class SubCategoryRepository : BaseRepository<SubCategoryEntity>, ISubCategoryRepository 
    {
        private readonly CosmeticStoreDbContext _context;
        public SubCategoryRepository(CosmeticStoreDbContext context, ILogger<SubCategoryRepository> logger) : base(context, logger)
        {
            _context = context;
        }

        protected override async Task<SubCategoryEntity?> DoGetByIdAsync(int id)
        {
            return await _context.SubCategories
                .Include(sc => sc.Category)
                .Include(sc => sc.ProductSubCategories)
                .ThenInclude(psc => psc.Product)
                .FirstOrDefaultAsync(sc => sc.Id == id);
        }

        protected override async Task<IEnumerable<SubCategoryEntity>> DoGetAllAsync()
        {
            return await _context.SubCategories
                .Include(sc => sc.Category)
                .Include(sc => sc.ProductSubCategories)
                .ThenInclude(psc => psc.Product)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task CreateWithProductsAsync(string name, int categoryId, List<int> productIds)
        {
            var subCategory = new SubCategoryEntity
            {
                Name = name,
                CategoryId = categoryId,
                ProductSubCategories = productIds.Select(pid => new ProductSubCategoryEntity { ProductId = pid }).ToList()
            };
            await _context.SubCategories.AddAsync(subCategory);
            await SaveChangesAsync();
        }

        public async Task<bool> UpdateWithProductsAsync(int id, string name, int categoryId, List<int> productIds)
        {
            var subCategory = await _context.SubCategories
                .Include(sc => sc.ProductSubCategories)
                .FirstOrDefaultAsync(sc => sc.Id == id);
            if (subCategory == null) return false;

            subCategory.Name = name;
            subCategory.CategoryId = categoryId;

            var existingProductIds = subCategory.ProductSubCategories.Select(psc => psc.ProductId).ToList();
            var productsToRemove = subCategory.ProductSubCategories.Where(psc => !productIds.Contains(psc.ProductId)).ToList();
            foreach (var psc in productsToRemove)
            {
                subCategory.ProductSubCategories.Remove(psc);
            }

            var newProductIds = productIds.Except(existingProductIds).ToList();
            foreach (var productId in newProductIds)
            {
                subCategory.ProductSubCategories.Add(new ProductSubCategoryEntity { ProductId = productId });
            }

            await SaveChangesAsync();
            return true;
        }
    }
}
