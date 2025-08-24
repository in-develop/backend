using Microsoft.EntityFrameworkCore;
using TeamChallenge.DbContext;
using TeamChallenge.Models.Entities;

namespace TeamChallenge.Repositories
{
    public class BaseRepository<T> : IRepository<T> where T : class, IEntity
    {
        private readonly CosmeticStoreDbContext _context;
        private readonly DbSet<T> _dbSet;
        
        public BaseRepository(CosmeticStoreDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }

        public virtual async Task<IEnumerable<T>> GetSortedAsync(Func<T, bool> filter)
        {
            return _dbSet.AsNoTracking().Where(filter).ToList();
        }

        public virtual async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual async Task CreateAsync(Action<T> entityFieldSetter)
        {
            var entity = Activator.CreateInstance<T>();
            entityFieldSetter(entity);

            await _dbSet.AddAsync(entity);
            await SaveChangesAsync();
        }

        public virtual async Task<bool> UpdateAsync(int id, Action<T> entityFieldSetter)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null)
            {
                return false;
            }
            
            entityFieldSetter(entity);
            await SaveChangesAsync();

            return true;
        }

        public virtual async Task<bool> DeleteAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);

            if (entity == null)
            {
                return false;
            }

            _dbSet.Remove(entity);
            await SaveChangesAsync();

            return true;
        }

        protected async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
