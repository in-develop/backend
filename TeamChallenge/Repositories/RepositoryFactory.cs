using TeamChallenge.DbContext;
using TeamChallenge.Models.Entities;

namespace TeamChallenge.Repositories
{
    public class RepositoryFactory
    {
        private  CosmeticStoreDbContext _context;
        private readonly Dictionary<Type, Type> _repositoryMapping = new Dictionary<Type, Type>
        {
            { typeof(ProductEntity), typeof(ProductRepository) },
            { typeof(CategoryEntity), typeof(CategoryRepository) },
        };

        public RepositoryFactory(CosmeticStoreDbContext context)
        {
            _context = context;
        }

        public IRepository<T> GetRepository<T>() where T : IEntity
        {
            if (_repositoryMapping.TryGetValue(typeof(T), out Type repositoryType))
            {
                return (IRepository<T>)Activator.CreateInstance(repositoryType, _context);
            }
            else
            {
                throw new InvalidOperationException($"No repository found for entity type: {typeof(T).Name}");
            }
        }
    }
}
