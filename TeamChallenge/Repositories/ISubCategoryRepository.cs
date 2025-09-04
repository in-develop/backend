using TeamChallenge.Models.Entities;

namespace TeamChallenge.Repositories
{
    public interface ISubCategoryRepository : IRepository<SubCategoryEntity>
    {
        Task CreateWithProductsAsync(string name, int categoryId, List<int> productIds);
        Task<bool> UpdateWithProductsAsync(int id, string name, int categoryId, List<int> productIds);
    }
}
