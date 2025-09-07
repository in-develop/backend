using TeamChallenge.DbContext;
using TeamChallenge.Models.Entities;

namespace TeamChallenge.Repositories
{
    public class ProductRepository : BaseRepository<ProductEntity>, IProductRepository
    {
        private readonly CosmeticStoreDbContext _context;
        public ProductRepository(CosmeticStoreDbContext context, ILogger<ProductRepository> logger) : base(context, logger) 
        {
            _context = context;
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
    }

}
