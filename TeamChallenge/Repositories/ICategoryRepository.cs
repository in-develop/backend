using TeamChallenge.Models.Entities;
namespace TeamChallenge.Repositories
{
    public interface ICategoryRepository : IRepository<CategoryEntity>
    {
        Task<CategoryEntity?> GetByIdWithSubCategoriesAsync(int id);
    }
}
