using TeamChallenge.DbContext;
using TeamChallenge.Interfaces.Product;
using TeamChallenge.Models.Entities;

namespace TeamChallenge.Repositories.Product
{
    public class ProductRepository : BaseRepository<ProductEntity>, IProductRepository
    {
        public ProductRepository(CosmeticStoreDbContext context) : base(context) 
        {
        }
    }
}
