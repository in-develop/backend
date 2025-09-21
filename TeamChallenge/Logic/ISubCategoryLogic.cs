using TeamChallenge.Models.Requests.SubCategory;
using TeamChallenge.Models.Responses;

namespace TeamChallenge.Logic
{
    public interface ISubCategoryLogic
    {
        Task<IResponse> GetAllSubCategoryAsync();
        Task<IResponse> GetSubCategoryByIdAsync(int id);
        Task<IResponse> CreateSubCategoryAsync(CreateSubCategoryRequest requestData);
        Task<IResponse> CreateMultipleSubCategoriesAsync(List<CreateSubCategoryManyRequest> requestData);

        Task<IResponse> UpdateSubCategoryAsync(int id, UpdateSubCategoryRequest requestData);
        Task<IResponse> DeleteSubCategoryAsync(int id);
    }
}
