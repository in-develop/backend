using TeamChallenge.Models.Entities;
using TeamChallenge.Models.Requests.Category;
using TeamChallenge.Models.Responses;
using TeamChallenge.Models.Responses.CategoryResponses;
using TeamChallenge.Models.Responses.SubCategoryResponses;
using TeamChallenge.Repositories;

namespace TeamChallenge.Logic
{
    public class CategoryLogic : ICategoryLogic
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ILogger<CategoryLogic> _logger;
        private readonly ISubCategoryRepository _subCategoryRepository;
        public CategoryLogic(RepositoryFactory factory, ILogger<CategoryLogic> logger)
        {
            _categoryRepository = (ICategoryRepository)factory.GetRepository<CategoryEntity>();
            _subCategoryRepository = (ISubCategoryRepository)factory.GetRepository<SubCategoryEntity>();
            _logger = logger;
        }

        public async Task<IResponse> GetAllCategoriesAsync()
        {
            try
            {
                var categories = await _categoryRepository.GetAllAsync();

                var response = categories.Select(c => new CategoryResponse
                {
                    Id = c.Id,
                    Name = c.Name,
                    SubCategories = c.SubCategories
                    .Select(sc => new SubCategoryResponse
                    {
                        Id = sc.Id,
                        Name = sc.Name
                    }).ToList()
                }).ToList();

                return new GetAllCategoriesResponse(response);
            }
            catch (Exception ex)
            {
                return new ServerErrorResponse(ex.Message);
            }
        }

        public async Task<IResponse> GetCategoryByIdAsync(int id)
        {
            try
            {
                var result = await _categoryRepository.GetByIdAsync(id);

                if (result == null)
                {
                    _logger.LogWarning("Category with Id = {id} not found.", id);
                    return new NotFoundResponse($"Category with Id={id} not found");
                }

                return new GetCategoryResponse(result);
            }
            catch (Exception ex)
            {
                return new ServerErrorResponse(ex.Message);
            }
        }

        public async Task<IResponse> CreateCategoryAsync(CreateCategoryRequest requestData)
        {
            try
            {

                await _categoryRepository.CreateAsync(entity =>
                {
                    entity.Name = requestData.Name;
                });

                if (requestData.SubCategories.Any())
                {
                    foreach (var subCategoryId in requestData.SubCategories)
                    {
                        var subCategory = await _subCategoryRepository.GetByIdAsync(subCategoryId);
                        if (subCategory != null)
                        {
                            await _subCategoryRepository.UpdateAsync(subCategory.Id, entity =>
                            {
                                entity.CategoryId = subCategoryId;
                            });
                        }
                    }
                }

                return new OkResponse();
            }
            catch (Exception ex)
            {
                return new ServerErrorResponse(ex.Message);
            }
        }

        public async Task<IResponse> UpdateCategoryAsync(int id, UpdateCategoryRequest requestData)
        {
            try
            {
                var result = await _categoryRepository.UpdateAsync(id, entity =>
                {
                    entity.Name = requestData.Name;
                });

                if (!result)
                {
                    _logger.LogWarning("Category with Id = {id} not found for update.", id);
                    return new NotFoundResponse($"Category with Id={id} not found");
                }

                return new OkResponse();
            }
            catch (Exception ex)
            {
                return new ServerErrorResponse(ex.Message);
            }
        }

        public async Task<IResponse> DeleteCategoryAsync(int id)
        {
            try
            {
                var result = await _categoryRepository.DeleteAsync(id);

                if (!result)
                {
                    _logger.LogWarning("Category with Id = {id} not found for deletion.", id);
                    return new ErrorResponse($"Category with Id={id} not found");
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
