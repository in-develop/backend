using TeamChallenge.DbContext;
using TeamChallenge.Models.Entities;

namespace TeamChallenge.Repositories
{
    public class ProductRepository : BaseRepository<ProductEntity>, IProductRepository
    {
        public ProductRepository(CosmeticStoreDbContext context, ILogger<ProductRepository> logger) : base(context, logger) 
        {
        }
    }

}
