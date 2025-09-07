using TeamChallenge.Models.Entities;
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

        public SubCategoryLogic(RepositoryFactory factory, ILogger<SubCategoryLogic> logger)
        {
            _subCategoryRepository = (ISubCategoryRepository)factory.GetRepository<SubCategoryEntity>();
            _logger = logger;
        }

        public async Task<IResponse> GetAllSubCategoriesAsync()
        {
            try
            {
                var result = await _subCategoryRepository.GetAllAsync();

                return new GetAllSubCategoriesResponse(result);
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

        public async Task<IResponse> CreateSubCategoryAsync(CreateSubCategoryRequest dto)
        {
            try
            {
                await _subCategoryRepository.CreateWithProductsAsync(dto.Name, dto.CategoryId, dto.ProductIds);
                return new OkResponse();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating subcategory with Name = {name}", dto.Name);
                return new ServerErrorResponse(ex.Message);
            }
        }

        public async Task<IResponse> UpdateSubCategoryAsync(int id, UpdateSubCategoryRequest dto)
        {
            try
            {
                var result = await _subCategoryRepository.UpdateWithProductsAsync(id, dto.Name, dto.CategoryId, dto.ProductIds);
                if (!result)
                {
                    _logger.LogWarning("SubCategory with Id = {id} not found for update.", id);
                    return new NotFoundResponse($"SubCategory with Id={id} not found");
                }
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
