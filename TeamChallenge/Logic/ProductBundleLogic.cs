using System.Reflection.Metadata;
using TeamChallenge.Models.Entities;
using TeamChallenge.Models.Requests;
using TeamChallenge.Models.Responses;
using TeamChallenge.Repositories;

namespace TeamChallenge.Logic
{
    public class ProductBundleLogic : IProductBundleLogic
    {
        private readonly IProductBundleRepository _bundleRepository;
        private readonly IProductRepository _productRepository;
        private readonly ILogger<ProductBundleLogic> _logger;

        public ProductBundleLogic(RepositoryFactory factory, ILogger<ProductBundleLogic> logger)
        {
            _bundleRepository = (IProductBundleRepository)factory.GetRepository<ProductBundleEntity>();
            _productRepository = (IProductRepository)factory.GetRepository<ProductEntity>();
            _logger = logger;
        }

        public async Task<IResponse> GetAllProductBundlesAsync()
        {
            try
            {
                var result = await _bundleRepository.GetAllAsync();
                return new GetAllProductBundlesResponse(result);
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
                var result = await _bundleRepository.GetByIdAsync(id);

                if (result == null)
                {
                    return new NotFoundResponse($"Product bundle with Id = {id} not found");
                }

                return new GetProductBundleResponse(result);
            }
            catch (Exception ex)
            {
                return new ServerErrorResponse(ex.Message);
            }
        }

        public async Task<IResponse> CreateProductBundleAsync(CreateProductBundleRequest requestData)
        {
            try
            {
                var bundle = await _bundleRepository.CreateAsync(entity =>
                {
                    entity.Name = requestData.Name;
                    entity.Description = requestData.Description;
                    entity.Price = requestData.Price;
                    entity.StockQuantity = requestData.StockQuantity;
                    entity.DiscountPrice = requestData.DiscountPrice;
                });

                await _productRepository.UpdateManyAsync(x => requestData.ProductIds.Contains(x.Id), entities =>
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

        public async Task<IResponse> UpdateProductBundleAsync(int id, UpdateProductBundleRequest requestData)
        {
            try
            {
                var result = await _bundleRepository.UpdateAsync(id, entity =>
                {
                    entity.Name = requestData.Name;
                    entity.Description = requestData.Description;
                    entity.Price = requestData.Price;
                    entity.StockQuantity = requestData.StockQuantity;
                    entity.DiscountPrice = requestData.DiscountPrice;
                });

                if (!result)
                {
                    return new NotFoundResponse($"Product bundle with Id = {id} not found");
                }

                result = await _productRepository.UpdateManyAsync(x => requestData.ProductIds.Contains(x.Id), entities =>
                {
                    entities.ForEach(e => e.ProductBundleId = id);
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

                return new OkResponse();
            }
            catch (Exception ex)
            {
                return new ServerErrorResponse(ex.Message);
            }
        }
    }
}
