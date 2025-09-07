using TeamChallenge.Models.Requests.SubCategory;
using TeamChallenge.Models.Responses;

namespace TeamChallenge.Logic
{
    public interface ISubCategoryLogic
    {
        Task<IResponse> GetAllSubCategoryAsync();
        Task<IResponse> GetSubCategoryByIdAsync(int id);
        Task<IResponse> CreateSubCategoryAsync(CreateSubCategoryRequest dto);
        Task<IResponse> UpdateSubCategoryAsync(int id, UpdateSubCategoryRequest dto);
        Task<IResponse> DeleteSubCategoryAsync(int id);
    }
}
