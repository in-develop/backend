using TeamChallenge.DbContext;
using TeamChallenge.Models.Entities;

namespace TeamChallenge.Repositories
{
    public class ProductBundleRepository : BaseRepository<ProductBundleEntity>, IProductBundleRepository
    {
        public ProductBundleRepository(CosmeticStoreDbContext context, ILogger<ProductBundleRepository> logger) : base(context, logger)
        {
        }
    }

}
