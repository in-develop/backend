using TeamChallenge.Models.Entities;
using TeamChallenge.Models.Requests;
using TeamChallenge.Models.Responses;
using TeamChallenge.Repositories;
using TeamChallenge.Services;

namespace TeamChallenge.Logic
{
    public class ProductBundleLogic : IProductBundleLogic
    {
        private readonly IProductBundleRepository _bundleRepository;
        private readonly IProductRepository _productRepository;
        private readonly IProductLogic _productLogic;
        private readonly ILogger<ProductBundleLogic> _logger;
        private readonly IRedisCacheService _cache;

        public ProductBundleLogic(
            RepositoryFactory factory, 
            IProductLogic productLogic,
            ILogger<ProductBundleLogic> logger,
            IRedisCacheService cache)
        {
            _bundleRepository = (IProductBundleRepository)factory.GetRepository<ProductBundleEntity>();
            _productRepository = (IProductRepository)factory.GetRepository<ProductEntity>();
            _logger = logger;
            _productLogic = productLogic;
            _cache = cache;
        }

        public async Task<IResponse> GetAllProductBundlesAsync()
        {
            try
            {
                var result = await _bundleRepository.GetAllAsync();
                return new GetAllProductBundlesResponse(result.Select(x => new GetProductBundleResponseModel(x)));
            }
            catch (Exception ex)
            {
                return new ServerErrorResponse(ex.Message);
            }
        }

        public async Task<IResponse> GetProductBundleByIdAsync(int id)
        {
            try
            {
                var cachedData = await _cache.GetValueAsync<GetProductBundleResponseModel>(id);

                if (cachedData != null)
                {
                    return new GetProductBundleResponse(cachedData);
                }

                var result = await _bundleRepository.GetByIdAsync(id);

                if (result == null)
                {
                    return new NotFoundResponse($"Product bundle with Id = {id} not found");
                }

                cachedData = new GetProductBundleResponseModel(result);
                await _cache.SetValueAsync(cachedData, id);

                return new GetProductBundleResponse(cachedData);
            }
            catch (Exception ex)
            {
                return new ServerErrorResponse(ex.Message);
            }
        }

        public async Task<IResponse> CreateProductBundleAsync(CreateProductBundleRequest request)
        {
            try
            {
                var response = await _productLogic.CheckIfProductsExists(request.ProductIds.ToArray());

                if (!response.IsSuccess)
                {
                    return response;
                }

                var bundle = await _bundleRepository.CreateAsync(entity =>
                {
                    entity.Name = request.Name;
                    entity.Description = request.Description;
                    entity.Price = request.Price;
                    entity.StockQuantity = request.StockQuantity;
                    entity.DiscountPrice = request.DiscountPrice;
                });

                await _productRepository.UpdateManyAsync(x => request.ProductIds.Contains(x.Id), entities =>
                {
                    entities.ForEach(e => e.ProductBundleId = bundle.Id);
                });

                return new OkResponse();
            }
            catch (Exception ex)
            {
                return new ServerErrorResponse(ex.Message);
            }
        }

        public async Task<IResponse> UpdateProductBundleAsync(int id, UpdateProductBundleRequest request)
        {
            try
            {
                var response = await _productLogic.CheckIfProductsExists(request.ProductIds.ToArray());

                if (!response.IsSuccess)
                {
                    return response;
                }

                var result = await _bundleRepository.UpdateAsync(id, entity =>
                {
                    entity.Name = request.Name;
                    entity.Description = request.Description;
                    entity.Price = request.Price;
                    entity.StockQuantity = request.StockQuantity;
                    entity.DiscountPrice = request.DiscountPrice;
                });

                if (!result)
                {
                    return new NotFoundResponse($"Product bundle with Id = {id} not found");
                }

                await _cache.RemoveValueAsync<GetProductBundleResponseModel>(id);

                result = await _productRepository.UpdateManyAsync(x => request.ProductIds.Contains(x.Id), entities =>
                {
                    entities.ForEach(async e =>
                    {
                        e.ProductBundleId = id;
                        await _cache.RemoveValueAsync<GetProductResponseModel>(e.Id);
                    });
                });

                if (!result)
                {
                    return new ConflictResponse($"Fail to update Product bundle with Id = {id}");
                }

                return new OkResponse();
            }
            catch (Exception ex)
            {
                return new ServerErrorResponse(ex.Message);
            }
        }

        public async Task<IResponse> DeleteProductBundleAsync(int id)
        {
            try
            {
                var result = await _bundleRepository.DeleteAsync(id);
                if (!result)
                {
                    return new NotFoundResponse($"Product bundle with Id = {id} not found");
                }

                await _cache.RemoveValueAsync<GetProductBundleResponseModel>(id);

                return new OkResponse();
            }
            catch (Exception ex)
            {
                return new ServerErrorResponse(ex.Message);
            }
        }
    }
}
