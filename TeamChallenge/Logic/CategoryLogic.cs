using TeamChallenge.Models.Entities;
using TeamChallenge.Models.Requests.Category;
using TeamChallenge.Models.Responses;
using TeamChallenge.Models.Responses.CategoryResponses;
using TeamChallenge.Repositories;

namespace TeamChallenge.Logic
{
    public class CategoryLogic : ICategoryLogic
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryLogic(RepositoryFactory factory)
        {
            _categoryRepository = (ICategoryRepository)factory.GetRepository<CategoryEntity>();
        }

        public async Task<IResponse> GetAllCategoriesAsync()
        {
            try
            {
                var result = await _categoryRepository.GetAllAsync();

                return new GetAllCategoriesResponse(result);
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
