using TeamChallenge.DbContext;
using TeamChallenge.Interfaces.Category;
using TeamChallenge.Models.Entities;
using TeamChallenge.Repositories;

public class CategoryRepository : BaseRepository<CategoryEntity>, ICategoryRepository
{
    public CategoryRepository(CosmeticStoreDbContext context) : base(context)
    {
    }
}