using TeamChallenge.Models.Entities;
using TeamChallenge.Repositories;
using TeamChallenge.Models.Responses;
using TeamChallenge.Models.Requests;

namespace TeamChallenge.Logic
{
    public class ProductLogic : IProductLogic
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<ProductLogic> _logger;

        public ProductLogic(RepositoryFactory factory, ILogger<ProductLogic> logger)
        {
            _productRepository = (IProductRepository)factory.GetRepository<ProductEntity>();
            _logger = logger;
        }

        public async Task<IResponse> GetAllProductsAsync()
        {
            try
            {
                var result = await _productRepository.GetAllAsync();
                return new GetAllProductsResponse(result);
            }
            catch (Exception ex)
            {
                return new ServerErrorResponse(ex.Message);
            }
        }

        public async Task<IResponse> GetProductByIdAsync(int id)
        {
            try
            {
                var result = await _productRepository.GetByIdAsync(id);

                if (result == null)
                {
                    return new NotFoundResponse($"Product with Id = {id} not found");
                }

                return new GetProductResponse(result);
            }
            catch (Exception ex)
            {
                return new ServerErrorResponse(ex.Message);
            }
        }

        public async Task<IResponse> CreateProductAsync(CreateProductRequest requestData)
        {
            try
            {
                await _productRepository.CreateAsync(entity =>
                {
                    entity.Name = requestData.Name;
                    entity.Description = requestData.Description;
                    entity.Price = requestData.Price;
                    entity.StockQuantity = requestData.StockQuantity;
                    entity.DiscountPrice = requestData.DiscountPrice;
                    entity.CategoryId = requestData.CategoryId;
                });

                return new OkResponse();
            }
            catch (Exception ex)
            {
                return new ServerErrorResponse(ex.Message);
            }
        }

        public async Task<IResponse> UpdateProductAsync(int id, UpdateProductRequest requestData)
        {
            try
            {
                var result = await _productRepository.UpdateAsync(id, entity =>
                {
                    entity.Name = requestData.Name;
                    entity.Description = requestData.Description;
                    entity.Price = requestData.Price;
                    entity.StockQuantity = requestData.StockQuantity;
                    entity.DiscountPrice = requestData.DiscountPrice;
                    entity.CategoryId = requestData.CategoryId;
                });

                if (!result)
                {
                    return new NotFoundResponse($"Product with Id = {id} not found");
                }

                return new OkResponse();
            }
            catch (Exception ex)
            {
                return new ServerErrorResponse(ex.Message);
            }
        }

        public async Task<IResponse> DeleteProductAsync(int id)
        {
            try
            {
                var result = await _productRepository.DeleteAsync(id);
                if (!result)
                {
                    return new NotFoundResponse($"Product with Id = {id} not found");
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
