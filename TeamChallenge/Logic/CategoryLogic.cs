using TeamChallenge.Models.Entities;
using TeamChallenge.Models.Models.Responses;
using TeamChallenge.Models.Models.Responses.SubCategoryResponse;
using TeamChallenge.Models.Requests;
using TeamChallenge.Models.Responses;
using TeamChallenge.Repositories;

namespace TeamChallenge.Logic
{
    public class CategoryLogic(RepositoryFactory factory, ILogger<CategoryLogic> logger) : ICategoryLogic
    {
        private readonly ICategoryRepository _categoryRepository = (ICategoryRepository)factory.GetRepository<CategoryEntity>();
        private readonly ISubCategoryRepository _subCategoryRepository = (ISubCategoryRepository)factory.GetRepository<SubCategoryEntity>();

        public async Task<IResponse> CheckIfCategoriesExists(params int[] categoryIDs)
        {
            var existingCategories = await _categoryRepository.GetFilteredAsync(p => categoryIDs.Contains(p.Id));
            var existingCategoryIds = existingCategories.Select(p => p.Id).ToHashSet();
            var missingCategoryIds = categoryIDs.Except(existingCategoryIds).ToList();

            if (missingCategoryIds.Any())
            {
                logger.LogWarning("Categories not found with IDs: {MissingProductIds}", string.Join(", ", missingCategoryIds));
                return new NotFoundResponse($"Categories not found with IDs: {string.Join(", ", missingCategoryIds)}");
            }

            return new OkResponse();
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
                logger.LogError(ex, "Error fetching all Categories.");
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
                    logger.LogWarning("Category with Id = {id} not found.", id);
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
                logger.LogError(ex, "An error occurred while creating a category.");
                return new ServerErrorResponse(ex.Message);
            }
        }

        public async Task<IResponse> CreateCategoryAsync(CreateCategoryRequest requestData)
        {
            try
            {
                if (requestData.SubCategoryIds != null && requestData.SubCategoryIds.Any())
                {
                    var existingSubCategories = await _subCategoryRepository.GetFilteredAsync(sc => requestData.SubCategoryIds.Contains(sc.Id));
                    var subCategoryEntities = existingSubCategories.ToList();
                    if (subCategoryEntities.Count != requestData.SubCategoryIds.Distinct().Count())
                    {
                        return new BadRequestResponse("One or more provided SubCategoryIds are invalid.");
                    }

                    await _categoryRepository.CreateAsync(entity =>
                    {
                        entity.Name = requestData.Name;
                        entity.SubCategories = subCategoryEntities.ToList();
                    });
                }
                else
                {
                    await _categoryRepository.CreateAsync(entity =>
                    {
                        entity.Name = requestData.Name;
                    });
                }

                return new OkResponse("Category has been successfully created")
                {
                    StatusCode = 201,
                };
            }
            catch (Exception ex)
            {
                return new ServerErrorResponse(ex.Message);
            }
        }

        public async Task<IResponse> CreateCategoryManyAsync(List<CreateCategoryManyRequest> requestData)
        {
            try
            {
                if (requestData == null || !requestData.Any())
                {
                    return new BadRequestResponse("The list of categories cannot be empty.");
                }

                int count = requestData.Count;

                await _categoryRepository.CreateManyAsync(count, entities =>
                {
                    for (int i = 0; i < count; i++)
                    {
                        entities[i].Name = requestData[i].Name;
                    }
                });

                return new OkResponse($"{count} categories created successfully.")
                {
                    StatusCode = 201
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while creating multiple categories.");
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
                //var idsToRemove = currentSubCategoryIds.Except(requestedSubCategoryIds).ToList();
                //if (idsToRemove.Any())
                //{
                //    var subCategoriesToRemove = categoryToUpdate.SubCategories
                //        .Where(sc => idsToRemove.Contains(sc.Id)).ToList();
                //    foreach (var subCategory in subCategoriesToRemove)
                //    {
                //        subCategory.CategoryId = null;
                //    }
                //}

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
                            return new BadRequestResponse($"SubCategory with Id={subCategoryId} not found");
                        }
                    }
                }

                return new OkResponse("Category updated successfully.");
            }
            catch (Exception ex)
            {
                return new ServerErrorResponse(ex.Message);
            }
        }

        public async Task<IResponse> UpdateCategoryManyAsync(List<UpdateCategoryManyRequest> requestData)
        {
            try
            {
                if (requestData == null || !requestData.Any())
                {
                    return new BadRequestResponse("The list of categories cannot be empty.");
                }

                var idsToUpdate = requestData.Select(r => r.Id).ToList();

                var wasSuccessful = await _categoryRepository.UpdateManyAsync(
                    category => idsToUpdate.Contains(category.Id), entities =>
                    {
                        var requestDict = requestData.ToDictionary(r => r.Id);
                        foreach (var entity in entities)
                        {
                            if (requestDict.TryGetValue(entity.Id, out var dto))
                            {
                                entity.Name = dto.Name;
                            }
                        }
                    });

                if (!wasSuccessful)
                {
                    return new NotFoundResponse("None of the specified categories were found.");
                }
                return new OkResponse("Categories were updated successfully.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while updating multiple categories.");
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
                    logger.LogWarning("Category with Id = {id} not found for deletion.", id);
                    return new NotFoundResponse($"Category with Id={id} not found");
                }

                return new OkResponse($"Category with Id = {id} was deleted.");
            }
            catch (Exception ex)
            {
                return new ServerErrorResponse(ex.Message);
            }
        }

        public async Task<IResponse> DeleteCategoryManyAsync(List<int> ids)
        {
            try
            {
                if (ids == null || !ids.Any())
                {
                    return new BadRequestResponse("No category IDs provided for deletion.");
                }
                var wasSuccessful = await _categoryRepository.DeleteManyAsync(c => ids.Contains(c.Id));
                if (!wasSuccessful)
                {
                    return new NotFoundResponse("None of the specified categories were found to delete.");
                }

                return new OkResponse("Categories deleted successfully.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while deleting multiple categories.");
                return new ServerErrorResponse(ex.Message);
            }
        }
    }
}
