using TeamChallenge.Models.Entities;
using TeamChallenge.Models.Models.Responses;
using TeamChallenge.Models.Models.Responses.SubCategoryResponse;
using TeamChallenge.Models.Requests.Category;
using TeamChallenge.Models.Responses;
using TeamChallenge.Models.Responses.CategoryResponses;
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
            _subCategoryRepository = (ISubCategoryRepository)factory.GetRepository<SubCategoryEntity>();
            _categoryRepository = (ICategoryRepository)factory.GetRepository<CategoryEntity>();
            _logger = logger;
        }

        public async Task<IResponse> GetAllCategoriesAsync()
        {
            try
            {
                var categories = await _categoryRepository.GetAllAsync();

                var categoryDtos = categories.Select(c => new CategoryResponse
                {
                    Id = c.Id,
                    Name = c.Name,
                    SubCategories = c.SubCategories.Select(sc => new SubCategoryResponse
                    {
                        Id = sc.Id,
                        Name = sc.Name
                    }).ToList()
                }).ToList();

                return new GetAllCategoriesResponse(categoryDtos);
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
                var categoryEntity = await _categoryRepository.GetByIdWithSubCategoriesAsync(id);

                if (categoryEntity == null)
                {
                    _logger.LogWarning("Category with Id = {id} not found.", id);
                    return new NotFoundResponse($"Category with Id={id} not found");
                }

                var categoryDto = new CategoryResponse
                {
                    Id = categoryEntity.Id,
                    Name = categoryEntity.Name,
                    SubCategories = categoryEntity.SubCategories.Select(sc => new SubCategoryResponse
                    {
                        Id = sc.Id,
                        Name = sc.Name,
                    }).ToList()
                };

                return new GetCategoryResponse(categoryDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a category.");
                return new ServerErrorResponse(ex.Message);
            }
        }

        public async Task<IResponse> CreateCategoryAsync(CreateCategoryRequest requestData)
        {
            try
            {

                var newCategory = new CategoryEntity
                {
                    Name = requestData.Name,
                    SubCategories = new List<SubCategoryEntity>()
                };

                if (requestData.SubCategoryIds != null && requestData.SubCategoryIds.Any())
                {
                    foreach (var subId in requestData.SubCategoryIds)
                    {
                        var subCategory = await _subCategoryRepository.GetByIdAsync(subId);
                        if (subCategory != null)
                        {
                            newCategory.SubCategories.Add(subCategory);
                        }
                        else
                        {
                            _logger.LogWarning("SubCategory with Id = {subId} not found while creating category. Skipping.", subId);
                            return new BadRequestResponse($"SubCategory with Id={subId} not found.");
                        }
                    }
                }

                await _categoryRepository.AddAsync(newCategory);
                await _categoryRepository.SaveChangesAsync();

                var categoryDto = new CategoryResponse
                {
                    Id = newCategory.Id,
                    Name = newCategory.Name,
                    SubCategories = newCategory.SubCategories.Select(sc => new SubCategoryResponse
                    {
                        Id = sc.Id,
                        Name = sc.Name
                    }).ToList()
                };

                return new CreateCategoryResponse(categoryDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating category with Name = {name}", requestData.Name);
                return new ServerErrorResponse(ex.Message);
            }
        }

        public async Task<IResponse> UpdateCategoryAsync(int id, UpdateCategoryRequest requestData)
        {
            try
            {
                // Load Category with its current Sub Categories
                var categoryToUpdate = await _categoryRepository.GetByIdWithSubCategoriesAsync(id);

                if (categoryToUpdate == null)
                {
                    return new NotFoundResponse($"Category with Id={id} not found");
                }

                // Update simple fields for Category
                categoryToUpdate.Name = requestData.Name;

                // Syncing Sub Categories
                var currentSubCategoryIds = categoryToUpdate.SubCategories.Select(sc => sc.Id).ToList();
                var requestedSubCategoryIds = requestData.SubCategoryIds;

                // Finding Sub Category Ids needed to unlink
                var idsToRemove = currentSubCategoryIds.Except(requestedSubCategoryIds).ToList();
                if (idsToRemove.Any())
                {
                    var subCategoriesToRemove = categoryToUpdate.SubCategories
                        .Where(sc => idsToRemove.Contains(sc.Id)).ToList();
                    foreach (var subCategory in subCategoriesToRemove)
                    {
                        subCategory.CategoryId = null;
                    }
                }

                // Finding Sub Category Ids needed to link
                var idsToAdd = requestedSubCategoryIds.Except(currentSubCategoryIds).ToList();
                if (idsToAdd.Any())
                {
                    foreach (var subCategoryId in idsToAdd)
                    {
                        var subCategoryToAdd = await _subCategoryRepository.GetByIdAsync(subCategoryId);
                        if (subCategoryToAdd != null)
                        {
                            subCategoryToAdd.CategoryId = categoryToUpdate.Id;
                        }
                        else
                        {
                            _logger.LogWarning("SubCategory with Id = {subCategoryId} not found. Skipping.", subCategoryId);
                            return new BadRequestResponse($"SubCategory with Id={subCategoryId} not found");
                        }
                    }
                }
                await _categoryRepository.SaveChangesAsync();

                return new OkResponse("Category updated successfully.");
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
