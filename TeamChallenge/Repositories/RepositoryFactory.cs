﻿using TeamChallenge.DbContext;
using TeamChallenge.Models.Entities;

namespace TeamChallenge.Repositories
{
    public class RepositoryFactory
    {
        private  CosmeticStoreDbContext _context;
        private ILoggerFactory _loggerFactory;
        private readonly Dictionary<Type, Type> _repositoryMapping = new Dictionary<Type, Type>
        {
            { typeof(ProductEntity), typeof(ProductRepository) },
            { typeof(CategoryEntity), typeof(CategoryRepository) },
            { typeof(ReviewEntity), typeof(ReviewRepository) },
            { typeof(CartEntity), typeof(CartRepository) },
            { typeof(CartItemEntity), typeof(CartItemRepository) }
        };

        private readonly Dictionary<string, List<CategoryEntity>> _ = new Dictionary<string, List<CategoryEntity>>();

        public RepositoryFactory(CosmeticStoreDbContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            _loggerFactory = loggerFactory;
        }

        public IRepository<T> GetRepository<T>() where T : IEntity
        {
            if (_repositoryMapping.TryGetValue(typeof(T), out Type repositoryType))
            {
                var createLoggerMethod = typeof(LoggerFactoryExtensions)
                    .GetMethod(nameof(LoggerFactoryExtensions.CreateLogger), [typeof(ILoggerFactory)])
                    .MakeGenericMethod(repositoryType);

                return (IRepository<T>)Activator.CreateInstance(
                    repositoryType, 
                    _context,
                    createLoggerMethod.Invoke(null, [_loggerFactory])
                    );
            }
            else
            {
                throw new InvalidOperationException($"No repository found for entity type: {typeof(T).Name}");
            }
        }
    }
}
