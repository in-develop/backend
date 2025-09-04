using TeamChallenge.Models.Requests.SubCategory;
using TeamChallenge.Models.Responses;

namespace TeamChallenge.Logic
{
    public interface ISubCategoryLogic
    {
        Task<IResponse> GetAllSubCategoriesAsync();
        Task<IResponse> GetSubCategoryByIdAsync(int id);
        Task<IResponse> CreateSubCategoryByAsync(CreateSubCategoryRequest dto);
        Task<IResponse> UpdateSubCategoryByAsync(int id, UpdateSubCategoryRequest dto);
        Task<IResponse> DeleteSubCategoryByAsync(int id);
    }
}
