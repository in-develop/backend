using TeamChallenge.DbContext;
using TeamChallenge.Models.Entities;

namespace TeamChallenge.Repositories
{
    public class RepositoryFactory
    {
        private  CosmeticStoreDbContext _context;
        private ILoggerFactory _loggerFactory;
        private ILogger<RepositoryFactory> _localLogger;
        private readonly Dictionary<Type, Type> _repositoryMapping = new Dictionary<Type, Type>
        {
            { typeof(ProductEntity), typeof(ProductRepository) },
            { typeof(ProductBundleEntity), typeof(ProductBundleRepository) },
            { typeof(CategoryEntity), typeof(CategoryRepository) },
            { typeof(ReviewEntity), typeof(ReviewRepository) },
            { typeof(CartEntity), typeof(CartRepository) },
            { typeof(OrderEntity), typeof(OrderRepository) },
            { typeof(OrderItemEntity), typeof(OrderItemRepository) },
            { typeof(OrderHistoryEntity), typeof(OrderHistoryRepository) },
            { typeof(CartItemEntity), typeof(CartItemRepository) }
        };

        private readonly Dictionary<string, List<CategoryEntity>> _ = new Dictionary<string, List<CategoryEntity>>();

        public RepositoryFactory(CosmeticStoreDbContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            _loggerFactory = loggerFactory;
            _localLogger = loggerFactory.CreateLogger<RepositoryFactory>();
        }

        public async Task<K> WrapWithTransactionAsync<K>(Func<Task<K>> action)
        {
            var transaction = await _context.Database.BeginTransactionAsync();
            _localLogger.LogInformation("Transaction started. ID : {0}", transaction.TransactionId);
            try
            {
                var result = await action();
                await transaction.CommitAsync();
                _localLogger.LogInformation("Transaction committed. ID : {0}", transaction.TransactionId);
                return result;
            }
            catch (Exception ex)
            {
                _localLogger.LogError(ex, "Transaction failed. ID : {0}", transaction.TransactionId);
                await transaction.RollbackAsync();
                throw;
            }

        }

        public IRepository<T> GetRepository<T>() where T : IEntity
        {
            if (_repositoryMapping.TryGetValue(typeof(T), out Type repositoryType))
            {
                var logger = typeof(LoggerFactoryExtensions)
                    .GetMethod(nameof(LoggerFactoryExtensions.CreateLogger), [typeof(ILoggerFactory)])
                    .MakeGenericMethod(repositoryType)
                    .Invoke(null, [_loggerFactory]);

                return (IRepository<T>)Activator.CreateInstance(
                    repositoryType, 
                    _context,
                    logger);
            }
            else
            {
                throw new InvalidOperationException($"No repository found for entity type: {typeof(T).Name}");
            }
        }
    }
}
