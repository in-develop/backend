namespace TeamChallenge.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity?> GetByIdAsync(int id);
        Task CreateAsync(TEntity entity);
        Task<bool> UpdateAsync(int id, TEntity entity);
        Task<bool> DeleteAsync(int id);
        Task SaveChangesAsync();
    }
}
