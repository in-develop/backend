using TeamChallenge.Models.Entities;
namespace TeamChallenge.Repositories
{
    public interface IProductRepository : IRepository<ProductEntity>
    {
        Task<int> CreateWithSubCategoriesAsync(string name, string? description, decimal price, List<int> SubCategoryIds);
    }
}
