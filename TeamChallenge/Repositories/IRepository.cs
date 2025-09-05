using TeamChallenge.Models.Entities;

namespace TeamChallenge.Repositories
{
    public interface IRepository<T> where T : IEntity
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetFilteredAsync(Func<T, bool> filter);
        Task<T?> GetByIdAsync(int id);
        Task<T> CreateAsync(Action<T> entityFieldSetter);
        Task<bool> UpdateAsync(int id, Action<T> entityFieldSetter);
        Task<bool> UpdateManyAsync(List<int> idList, Action<T> entityFieldSetter);
        Task<bool> DeleteAsync(int id);
    }
}
