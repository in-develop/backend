using TeamChallenge.Models.Requests.Category;
using TeamChallenge.Models.Responses;

namespace TeamChallenge.Logic
{
    public interface ISubCategoryLogic
    {
        Task<IResponse> GetAllSubCategoriesAsync();
        Task<IResponse> GetSubCategoryByIdAsync(int id);
        Task<IResponse> CreateSubCategoryByAsync(CreateCategoryRequest dto);
        Task<IResponse> UpdateSubCategoryByAsync(int id, UpdateCategoryRequest dto);
        Task<IResponse> DeleteSubCategoryByAsync(int id);
    }
}
