using Microsoft.EntityFrameworkCore;
using TeamChallenge.DbContext;
using TeamChallenge.Models.Entities;

namespace TeamChallenge.Repositories
{
    public class ProductBundleRepository : BaseRepository<ProductBundleEntity>, IProductBundleRepository
    {
        public ProductBundleRepository(CosmeticStoreDbContext context, ILogger<ProductBundleRepository> logger) : base(context, logger)
        {
        }

        protected async override Task<ProductBundleEntity?> DoGetByIdAsync(int id)
        {
            var result = await _dbSet.Include(x => x.Products).FirstOrDefaultAsync(x => x.Id == id);

            if (result == null)
            {
                _logger.LogWarning("Entity of type {0} with ID = {1} not found.", typeof(ProductBundleEntity).Name, id);
            }

            return result;
        }
    }

}
