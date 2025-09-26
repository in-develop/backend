using TeamChallenge.Models.Requests;
using TeamChallenge.Models.Responses;

namespace TeamChallenge.Logic
{
    public interface ICategoryLogic
    {
        Task<IResponse> CheckIfCategoriesExists(params int[] categoryIDs);
        Task<IResponse> GetAllCategoriesAsync();
        Task<IResponse> GetCategoryByIdAsync(int id);
        Task<IResponse> CreateCategoryAsync(CreateCategoryRequest dto);
        Task<IResponse> CreateCategoryManyAsync(List<CreateCategoryManyRequest> requestData);
        Task<IResponse> UpdateCategoryAsync(int id, UpdateCategoryRequest dto);
        Task<IResponse> UpdateCategoryManyAsync(List<UpdateCategoryManyRequest> requestData);
        Task<IResponse> DeleteCategoryAsync(int id);
        Task<IResponse> DeleteCategoryManyAsync(List<int> ids);
    }
}
