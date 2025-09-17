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
        private readonly IProductLogic _productLogic;
        private readonly ILogger<ProductBundleLogic> _logger;

        public ProductBundleLogic(
            RepositoryFactory factory, 
            IProductLogic productLogic,
            ILogger<ProductBundleLogic> logger)
        {
            _bundleRepository = (IProductBundleRepository)factory.GetRepository<ProductBundleEntity>();
            _productRepository = (IProductRepository)factory.GetRepository<ProductEntity>();
            _logger = logger;
            _productLogic = productLogic;
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
                var result = await _bundleRepository.GetByIdAsync(id);

                if (result == null)
                {
                    return new NotFoundResponse($"Product bundle with Id = {id} not found");
                }

                return new GetProductBundleResponse(new GetProductBundleResponseModel(result));
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

                result = await _productRepository.UpdateManyAsync(x => request.ProductIds.Contains(x.Id), entities =>
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
