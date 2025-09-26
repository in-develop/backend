using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TeamChallenge.DbContext;
using TeamChallenge.Models.Entities;

namespace TeamChallenge.Repositories
{
    public class ProductRepository : BaseRepository<ProductEntity>, IProductRepository
    {
        private readonly CosmeticStoreDbContext _context;
        public ProductRepository(CosmeticStoreDbContext context, ILogger<ProductRepository> logger) : base(context, logger)
        {
        }


        protected override async Task<bool> DoDeleteAsync(int id)
        {
            var entity = await _dbSet.Include(p => p.ProductSubCategories).FirstOrDefaultAsync(p => p.Id == id);

            if (entity == null)
            {
                _logger.LogWarning("Entity type of {0} with ID = {1} not found.", typeof(ProductEntity).Name, id);
            }

            _dbSet.Remove(entity);
            await SaveChangesAsync();

            return true;
        }

        protected override async Task<bool> DoDeleteManyAsync(Expression<Func<ProductEntity, bool>> filter)
        {
            var entities = await _dbSet.Include(p => p.ProductSubCategories)
                .Where(filter)
                .ToListAsync();

            if (!entities.Any())
            {
                return false;
            }

            _dbSet.RemoveRange(entities);
            await SaveChangesAsync();

            return true;
        }

        public async Task<int> CreateWithSubCategoriesAsync(string name, string? description, decimal price, List<int> subCategoryIds)
        {
            var product = new ProductEntity
            {
                Name = name,
                Description = description,
                Price = price,
                ProductSubCategories = subCategoryIds.Select(scId => new ProductSubCategoryEntity
                {
                    SubCategoryId = scId,
                }).ToList()
            };
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return product.Id;
        }

        public async Task<IEnumerable<ProductEntity>> GetAllWithSubCategoriesAsync()
        {
            return await _dbSet
                .Include(p => p.ProductSubCategories)
                .ThenInclude(psc => psc.SubCategory)
                .ToListAsync();
        }

        public async Task<ProductEntity?> GetByIdWithSubCategoriesAsync(int id)
        {
            return await _dbSet
                .Include(p => p.ProductSubCategories)
                .ThenInclude(psc => psc.SubCategory)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
    }

}
