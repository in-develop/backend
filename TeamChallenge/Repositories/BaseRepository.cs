using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TeamChallenge.DbContext;
using TeamChallenge.Models.Entities;

namespace TeamChallenge.Repositories
{
    public class BaseRepository<T> : IRepository<T> where T : class, IEntity
    {
        private readonly CosmeticStoreDbContext _context;
        protected readonly DbSet<T> _dbSet;
        protected readonly ILogger<IRepository<T>> _logger;

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

        public async Task<IEnumerable<T>> GetFilteredAsync(Expression<Func<T, bool>> filter)
        {
            _logger.LogInformation("Fetching filtered records of type {0}", typeof(T).Name);
            return await DoGetFilteredAsync(filter);
        }

        protected virtual async Task<IEnumerable<T>> DoGetFilteredAsync(Expression<Func<T, bool>> filter)
        {
            return await _dbSet.AsNoTracking().Where(filter).ToListAsync();
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
        public async Task CreateManyAsync(int count, Action<List<T>> entityFieldSetter)
        {
            _logger.LogInformation("Creating {0} new records of type {1}", count, typeof(T).Name);
            await DoCreateManyAsync(count, entityFieldSetter);
        }
        protected async Task DoCreateManyAsync(int count, Action<List<T>> entityFieldSetter)
        {
            var entities = Enumerable.Range(0, count).Select(_ => Activator.CreateInstance<T>()).ToList();
            entityFieldSetter(entities);

            await _dbSet.AddRangeAsync(entities);
            await SaveChangesAsync();
            
            return entity;
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

        public async Task<bool> UpdateManyAsync(Expression<Func<T, bool>> filter, Action<List<T>> entityFieldSetter)
        {
            _logger.LogInformation("Updating multiple records of type {0}", typeof(T).Name);
            return await DoUpdateManyAsync(filter, entityFieldSetter);
        }

        protected virtual async Task<bool> DoUpdateManyAsync(Expression<Func<T, bool>> filter, Action<List<T>> entityFieldSetter)
        {
            var entities = await _dbSet.AsNoTracking().Where(filter).ToListAsync();

            if (entities.Count == 0)
            {
                _logger.LogWarning("No records found for update of type {0}", typeof(T).Name);
                return false;
            }

            entityFieldSetter(entities);
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

        public async Task<bool> DeleteManyAsync(Expression<Func<T, bool>> filter)
        {
            _logger.LogInformation("Deleting entities of type {0}", typeof(T).Name);
            return await DoDeleteManyAsync(filter);
        }

        protected virtual async Task<bool> DoDeleteManyAsync(Expression<Func<T, bool>> filter)
        {
            var entities = await _dbSet.AsNoTracking().Where(filter).ToListAsync();

            _dbSet.RemoveRange(entities);

            _logger.LogInformation("Entities deleted. IDs : {0}", string.Join(", ", entities.Select(x => x.Id)));

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
