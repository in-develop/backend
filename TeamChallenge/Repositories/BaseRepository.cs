using Microsoft.EntityFrameworkCore;
using TeamChallenge.DbContext;
using TeamChallenge.Models.Entities;

namespace TeamChallenge.Repositories
{
    public class BaseRepository<T> : IRepository<T> where T : class, IEntity
    {
        private readonly CosmeticStoreDbContext _context;
        private readonly DbSet<T> _dbSet;
        private readonly ILogger<IRepository<T>> _logger;

        public BaseRepository(CosmeticStoreDbContext context, ILogger<IRepository<T>> logger)
        {
            _context = context;
            _dbSet = _context.Set<T>();
            _logger = logger;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            _logger.LogInformation("Fetching all records of type {0}", typeof(T).Name);
            return await DoGetAllAsync();
        }

        protected virtual async Task<IEnumerable<T>> DoGetAllAsync()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetFilteredAsync(Func<T, bool> filter)
        {
            _logger.LogInformation("Fetching filtered records of type {0}", typeof(T).Name);
            return await DoGetFilteredAsync(filter);
        }
        protected virtual async Task<IEnumerable<T>> DoGetFilteredAsync(Func<T, bool> filter)
        {
            return _dbSet.AsNoTracking().Where(filter).ToList();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            _logger.LogInformation("Fetching record of type {0} with ID = {1}", typeof(T).Name, id);
            return await DoGetByIdAsync(id);
        }

        protected virtual async Task<T?> DoGetByIdAsync(int id)
        {
            var result = await _dbSet.FindAsync(id);

            if(result == null)
            {
                _logger.LogWarning("Entity of type {0} with ID = {1} not found.", typeof(T).Name, id);
            }

            return result;
        }

        public async Task<T> CreateAsync(Action<T> entityFieldSetter)
        {
            _logger.LogInformation("Creating a new record of type {0}", typeof(T).Name);
            return await DoCreateAsync(entityFieldSetter);
        }

        protected virtual async Task<T> DoCreateAsync(Action<T> entityFieldSetter)
        {
            var newEntity = Activator.CreateInstance<T>();
            entityFieldSetter(newEntity);

            await _dbSet.AddAsync(newEntity);
            await SaveChangesAsync();

            return newEntity;
        }

        public async Task<bool> UpdateAsync(int id, Action<T> entityFieldSetter)
        {
            _logger.LogInformation("Updating record of type {0} with ID = {1}", typeof(T).Name, id);
            return await DoUpdateAsync(id, entityFieldSetter);
        }

        protected virtual async Task<bool> DoUpdateAsync(int id, Action<T> entityFieldSetter)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null)
            {
                _logger.LogWarning("Entity of type {0} with ID = {1} not found for update.", typeof(T).Name, id);
                return false;
            }

            entityFieldSetter(entity);
            await SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateManyAsync(List<int> idList, Action<T> entityFieldSetter)
        {
            _logger.LogInformation("Updating multiple records of type {0} with ID's = {1}", typeof(T).Name, string.Join(",", idList));
            return await DoUpdateAsync(idList, entityFieldSetter);
        }

        protected virtual async Task<bool> DoUpdateAsync(List<int> idList, Action<T> entityFieldSetter)
        {
            var entities = await _dbSet.Where(x => idList.Contains(x.Id))
                .ToDictionaryAsync(x => x.Id, x => x);

            foreach (var id in idList)
            {
                if (!entities.TryGetValue(id, out var entity))
                {
                    _logger.LogWarning("Entity of type {0} with ID = {1} not found for update.", typeof(T).Name, id);
                    return false;
                }
                entityFieldSetter(entity);
            }

            await SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            _logger.LogInformation("Deleting record of type {0} with ID = {1}", typeof(T).Name, id);
            return await DoDeleteAsync(id);

        }

        protected virtual async Task<bool> DoDeleteAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);

            if (entity == null)
            {
                _logger.LogWarning("Entity of type {0} with ID = {1} not found.", typeof(T).Name, id);
                return false;
            }

            _dbSet.Remove(entity);
            await SaveChangesAsync();

            return true;
        }

        protected async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
            _logger.LogInformation("Changes saved to the database.");
        }
    }
}
