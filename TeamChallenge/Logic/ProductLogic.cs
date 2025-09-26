using TeamChallenge.Models.Entities;
using TeamChallenge.Repositories;
using TeamChallenge.Models.Responses;
using TeamChallenge.Models.Requests;
using SQLitePCL;
using TeamChallenge.DbContext;
using TeamChallenge.Services;

namespace TeamChallenge.Logic
{
    public class ProductLogic : IProductLogic
    {
        private readonly IProductRepository _productRepository;
        private readonly CosmeticStoreDbContext _context;
        private readonly ILogger<ProductLogic> _logger;
        private readonly ICategoryLogic _categoryLogic;
        private readonly IRedisCacheService _cache;

        public ProductLogic(
            RepositoryFactory factory, 
            ILogger<ProductLogic> logger, 
            ICategoryLogic categoryLogic,
            IRedisCacheService cache)
        {
            _productRepository = (IProductRepository)factory.GetRepository<ProductEntity>();
            _logger = logger;
            _categoryLogic = categoryLogic;
            _cache = cache;
        }

        public async Task<IResponse> DeleteManyProductsAsync(List<int> ids)
        {
            try
            {
                await _productRepository.DeleteManyAsync(p => ids.Contains(p.Id));

                return new OkResponse("Products deleted successfully.");
            }
            catch (Exception ex)
            {
                return new ServerErrorResponse(ex.Message);
            }
        }

        public async Task<IResponse> CheckIfProductsExists(params int[] productIds)
        {
            var existingProducts = await _productRepository.GetFilteredAsync(p => productIds.Contains(p.Id));
            var existingProductIds = existingProducts.Select(p => p.Id).ToHashSet();
            var missingProductIds = productIds.Except(existingProductIds).ToList();

            if (missingProductIds.Any())
            {
                _logger.LogWarning("Products not found with IDs: {MissingProductIds}", string.Join(", ", missingProductIds));
                return new NotFoundResponse($"Products not found with IDs: {string.Join(", ", missingProductIds)}");
            }

            return new OkResponse();
        }

        public async Task<IResponse> GetAllProductsAsync()
        {
            try
            {
                var result = await _productRepository.GetAllAsync();
                return new GetAllProductsResponse(result.Select(x => new GetProductResponseModel(x)));
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
                var result = await _cache.GetValueAsync<ProductEntity>(id);

                if (result != null)
                {
                    return new GetProductResponse(new GetProductResponseModel(result));
                }

                result = await _productRepository.GetByIdAsync(id);

                if (result == null)
                {
                    return new NotFoundResponse($"Product with Id = {id} not found");
                }

                await _cache.SetValueAsync(result, id);

                return new GetProductResponse(new GetProductResponseModel(result));
            }
            catch (Exception ex)
            {
                return new ServerErrorResponse(ex.Message);
            }
        }

        public async Task<IResponse> CreateProductAsync(CreateProductRequest requestData)
        {
            return new OkResponse();
        }
        // try
            // {
            //     if (requestData.SubCategories.Any())
            //     {
            //         entity.Name = requestData.Name;
            //         entity.Description = requestData.Description;
            //         entity.Price = requestData.Price;
            //         entity.StockQuantity = requestData.StockQuantity;
            //         entity.DiscountPrice = requestData.DiscountPrice;
            //         entity.CategoryId = requestData.CategoryId;
            //     };
            //
            //     var productId = await _productRepository.CreateWithSubCategoriesAsync(requestData.Name, requestData.Description, requestData.Price, requestData.SubCategories);
            //     _logger.LogInformation("Successfuly created product with Id = {id} and Name = {name}", productId, requestData);
            //     return new OkResponse();
            // }
            // catch (Exception ex)
            // {
            //     _logger.LogError(ex, "Error creating product with Name = {name}", requestData.Name);
            //     return new ServerErrorResponse(ex.Message);
            // }
        // }
        //{
        //    try
        //    {
        //        await _productRepository.CreateAsync(entity =>
        //        {
        //            entity.Name = requestData.Name;
        //            entity.Description = requestData.Description;
        //            entity.Price = requestData.Price;
        //            //entity.CategoryId = requestData.CategoryId;
        //        });

        //        return new OkResponse();
        //    }
        //    catch (Exception ex)
        //    {
        //        return new ServerErrorResponse(ex.Message);
        //    }
        //}

        public async Task<IResponse> UpdateProductAsync(int id, UpdateProductRequest requestData)
        {
            try
            {
                var response = await _categoryLogic.CheckIfCategoriesExists(requestData.CategoryId);

                if (!response.IsSuccess)
                {
                    return response;
                }

                var result = await _productRepository.UpdateAsync(id, entity =>
                {
                    entity.Name = requestData.Name;
                    entity.Description = requestData.Description;
                    entity.Price = requestData.Price;
                    entity.StockQuantity = requestData.StockQuantity;
                    entity.DiscountPrice = requestData.DiscountPrice;
                    //entity.CategoryId = requestData.CategoryId;
                });

                if (!result)
                {
                    return new NotFoundResponse($"Product with Id = {id} not found");
                }

                await _cache.RemoveValueAsync<ProductEntity>(id);

                return response;
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

                await _cache.RemoveValueAsync<ProductEntity>(id);

                return new OkResponse();
            }
            catch (Exception ex)
            {
                return new ServerErrorResponse(ex.Message);
            }
        }
    }
}
