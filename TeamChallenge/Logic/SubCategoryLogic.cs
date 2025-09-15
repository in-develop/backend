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
                var subCategories = await _subCategoryRepository.GetAllAsync();
                var dtos = subCategories.Select(sc => new SubCategoryResponse
                {
                    Id = sc.Id,
                    Name = sc.Name,
                }).ToList();

                return new GetAllSubCategoriesResponse(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all SubCategories");
                return new ServerErrorResponse(ex.Message);
            }
        }

        public async Task<IResponse> GetSubCategoryByIdAsync(int id)
        {
            try
            {
                var subCategory = await _subCategoryRepository.GetByIdAsync(id);

                if (subCategory == null)
                {
                    _logger.LogWarning($"SubCategory with Id = {id} not found", id);
                    return new NotFoundResponse($"Sub Category with Id = {id} not found");
                }

                var dto = new SubCategoryResponse
                {
                    Id = subCategory.Id,
                    Name = subCategory.Name,
                };

                return new GetSubCategoryResponse(dto);
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

                var result = await _subCategoryRepository.CreateAsync(entity =>
                {
                    entity.Name = requestData.Name;
                    entity.CategoryId = requestData.CategoryId;
                });

                var subCategoryDto = new SubCategoryResponse
                {
                    Id = result.Id,
                    Name = result.Name,
                    CategoryId = requestData.CategoryId,
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

                var subCategory = await _subCategoryRepository.GetByIdAsync(id);

                var result = await _subCategoryRepository.UpdateAsync(id, entity =>
                {
                    entity.Name = requestData.Name;
                });

                return new OkResponse("SubCategory updated successfully.");
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
