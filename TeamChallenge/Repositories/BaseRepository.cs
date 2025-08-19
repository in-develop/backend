using Microsoft.EntityFrameworkCore;
using TeamChallenge.DbContext;
using TeamChallenge.Interfaces;

namespace TeamChallenge.Repositories
{
    public class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly CosmeticStoreDbContext _context;
        private readonly DbSet<TEntity> _dbSet;
        
        public BaseRepository(CosmeticStoreDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>(); // Read Docs in details
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<TEntity?> GetByIdAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            return entity;
        }

        public async Task AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task<bool> UpdateAsync(int id, TEntity updateEntity)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null) return false;
            _context.Entry(entity).CurrentValues.SetValues(updateEntity);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null) return false;
            _dbSet.Remove(entity);
            return true;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
