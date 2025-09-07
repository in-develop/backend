using TeamChallenge.Models.Entities;

namespace TeamChallenge.Repositories
{
    public interface ISubCategoryRepository : IRepository<SubCategoryEntity>
    {
        Task<IEnumerable<SubCategoryEntity>> GetAllWithCategoryAsync();
        Task<SubCategoryEntity?> GetByIdWithDetailsAsync(int id);
    }
}
