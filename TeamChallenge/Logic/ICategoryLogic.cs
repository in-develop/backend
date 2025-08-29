using TeamChallenge.Models.Requests.Category;
using TeamChallenge.Models.Responses;

namespace TeamChallenge.Logic
{
    public interface ICategoryLogic
    {
        Task<IResponse> GetAllCategoriesAsync();
        Task<IResponse> GetCategoryByIdAsync(int id);
        Task<IResponse> CreateCategoryAsync(CreateCategoryRequest dto);
        Task<IResponse> UpdateCategoryAsync(int id, UpdateCategoryRequest dto);
        Task<IResponse> DeleteCategoryAsync(int id);
    }
}
