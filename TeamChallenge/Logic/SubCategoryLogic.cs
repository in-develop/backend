using Azure.Core;
using TeamChallenge.Models.Entities;
using TeamChallenge.Models.Models.Responses.SubCategoryResponse;
using TeamChallenge.Models.Requests.SubCategory;
using TeamChallenge.Models.Responses;
using TeamChallenge.Models.Responses.SubCategoryResponses;
using TeamChallenge.Repositories;

namespace TeamChallenge.Logic
{
    public class SubCategoryLogic : ISubCategoryLogic
    {
        private readonly ISubCategoryRepository _subCategoryRepository;
        private readonly ILogger<SubCategoryLogic> _logger;
        private readonly ICategoryRepository _categoryRepository;

        public SubCategoryLogic(RepositoryFactory factory, ILogger<SubCategoryLogic> logger)
        {
            _subCategoryRepository = (ISubCategoryRepository)factory.GetRepository<SubCategoryEntity>();
            _logger = logger;
            _categoryRepository = (ICategoryRepository)factory.GetRepository<CategoryEntity>();
        }

        public async Task<IResponse> GetAllSubCategoryAsync()
        {
            try
            {
                var subCategories = await _subCategoryRepository.GetAllWithCategoryAsync();
                var dtos = subCategories.Select(sc => new SubCategoryResponse
                {
                    Id = sc.Id,
                    Name = sc.Name,
                    Category = new ParentCategoryResponse { Id = sc.Category.Id, Name = sc.Category.Name }
                }).ToList();

                return new GetAllSubCategoriesResponse(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all subcategories");
                return new ServerErrorResponse(ex.Message);
            }
        }

        public async Task<IResponse> GetSubCategoryByIdAsync(int id)
        {
            try
            {
                var result = await _subCategoryRepository.GetByIdAsync(id);

                if (result == null)
                {
                    _logger.LogWarning($"Sub Category with Id = {id} not found", id);
                    return new NotFoundResponse($"Sub Category with Id = {id} not found");
                }

                return new GetSubCategoryResponse(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching subcategory with Id = {id}", id);
                return new ServerErrorResponse(ex.Message);
            }
        }

        public async Task<IResponse> CreateSubCategoryAsync(CreateSubCategoryRequest requestData)
        {
            try
            {
                var parentCategory = await _categoryRepository.GetByIdAsync(requestData.CategoryId);
                if (parentCategory == null)
                {
                    return new BadRequestResponse($"Category with Id={requestData.CategoryId} does not exist.");
                }

                var newSubCategory = new SubCategoryEntity
                {
                    Name = requestData.Name,
                    CategoryId = requestData.CategoryId
                };

                await _subCategoryRepository.AddAsync(newSubCategory);
                await _subCategoryRepository.SaveChangesAsync();

                var subCategoryDto = new SubCategoryResponse
                {
                    Id = newSubCategory.Id,
                    Name = newSubCategory.Name,
                    Category = new ParentCategoryResponse { Id = parentCategory.Id, Name = parentCategory.Name }
                };

                return new CreateSubCategoryResponse(subCategoryDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating subcategory with Name = {name}", requestData.Name);
                return new ServerErrorResponse(ex.Message);
            }
        }

        public async Task<IResponse> UpdateSubCategoryAsync(int id, UpdateSubCategoryRequest requestData)
        {
            try
            {
                //var result = await _subCategoryRepository.UpdateWithProductsAsync(id, dto.Name, dto.CategoryId, dto.ProductIds);
                //if (!result)
                //{
                //    _logger.LogWarning("SubCategory with Id = {id} not found for update.", id);
                //    return new NotFoundResponse($"SubCategory with Id={id} not found");
                //}
                return new OkResponse();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating subcategory with Id = {id}", id);
                return new ServerErrorResponse(ex.Message);
            }
        }

        public async Task<IResponse> DeleteSubCategoryAsync(int id)
        {
            try
            {
                var result = await _subCategoryRepository.DeleteAsync(id);
                if (!result)
                {
                    _logger.LogWarning("SubCategory with Id = {id} not found for deletion.", id);
                    return new NotFoundResponse($"SubCategory with Id={id} not found");
                }
                return new OkResponse();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting subcategory with Id = {id}", id);
                return new ServerErrorResponse(ex.Message);
            }
        }

    }
}
