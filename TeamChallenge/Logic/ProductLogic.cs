using TeamChallenge.Models.Entities;
using TeamChallenge.Models.Requests.Product;
using TeamChallenge.Models.Responses;
using TeamChallenge.Models.Responses.ProductResponses;
using TeamChallenge.Repositories;
using TeamChallenge.Services;

namespace TeamChallenge.Logic
{
    public class ProductLogic : IProductLogic
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<ProductLogic> _logger;
        private readonly ISubCategoryRepository _subCategoryRepository;
        private readonly IRedisCacheService _cache;

        public ProductLogic(
            RepositoryFactory factory,
            ILogger<ProductLogic> logger,
            ICategoryLogic categoryLogic,
            IRedisCacheService cache)
        {
            _productRepository = (IProductRepository)factory.GetRepository<ProductEntity>();
            _subCategoryRepository = (ISubCategoryRepository)factory.GetRepository<SubCategoryEntity>();
            _logger = logger;
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
                return new GetAllProductsResponse(result.Select(x => new ProductSummaryResponseModel(x)));
            }
            catch (Exception ex)
            {
                return new ServerErrorResponse(ex.Message);
            }
        }

        // Looping error
        public async Task<IResponse> GetProductByIdAsync(int id)
        {
            try
            {
                var cachedData = await _cache.GetValueAsync<GetProductResponseModel>(id);

                if (cachedData != null)
                {
                    return new GetProductResponse(cachedData);
                }

                var result = await _productRepository.GetByIdWithSubCategoriesAsync(id);

                if (result == null)
                {
                    return new NotFoundResponse($"Product with Id = {id} not found");
                }

                cachedData = new GetProductResponseModel(result);
                await _cache.SetValueAsync(cachedData, id);

                return new GetProductResponse(cachedData);
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
                var existingSubCategories = await _subCategoryRepository.GetFilteredAsync(sc => requestData.SubCategoryIds.Contains(sc.Id));

                if (existingSubCategories.Count() != requestData.SubCategoryIds.Distinct().Count())
                {
                    return new BadRequestResponse("One or more provided SubCategoryIds are invalid.");
                }

                await _productRepository.CreateAsync(entity =>
                {
                    entity.Name = requestData.Name;
                    entity.Description = requestData.Description;
                    entity.Volume = requestData.Volume;
                    entity.Price = requestData.Price;
                    entity.DiscountPrice = requestData.DiscountPrice;
                    entity.StockQuantity = requestData.StockQuantity;

                    foreach (var subId in requestData.SubCategoryIds)
                    {
                        entity.ProductSubCategories.Add(new ProductSubCategoryEntity { SubCategoryId = subId });
                    }
                });
                return new OkResponse("Product successfully created") { StatusCode = 201 };
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
                var productToUpdate = await _productRepository.GetByIdWithSubCategoriesAsync(id);

                if (productToUpdate == null)
                {
                    return new NotFoundResponse($"Product with Id = {id} not found");
                }

                var existingSubCategories = await _subCategoryRepository.GetFilteredAsync(sc => requestData.SubCategoryIds.Contains(sc.Id));

                if (existingSubCategories.Count() != requestData.SubCategoryIds.Distinct().Count())
                {
                    return new BadRequestResponse("One or more provided SubCategoryIds are invalid.");
                }

                await _productRepository.UpdateAsync(id, entity =>
                {
                    entity.Name = requestData.Name;
                    entity.Description = requestData.Description;
                    entity.Volume = requestData.Volume;
                    entity.Price = requestData.Price;
                    entity.DiscountPrice = requestData.DiscountPrice;
                    entity.StockQuantity = requestData.StockQuantity;

                    entity.ProductSubCategories.Clear();
                    foreach (var subId in requestData.SubCategoryIds)
                    {
                        entity.ProductSubCategories.Add(new ProductSubCategoryEntity { SubCategoryId = subId });
                    }
                });

                await _cache.RemoveValueAsync<GetProductResponseModel>(id);

                return new OkResponse("Product was successfully updated");
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

                await _cache.RemoveValueAsync<GetProductResponseModel>(id);

                return new OkResponse();
            }
            catch (Exception ex)
            {
                return new ServerErrorResponse(ex.Message);
            }
        }
    }
}
