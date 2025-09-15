using TeamChallenge.Models.Requests;
using TeamChallenge.Models.Responses;


namespace TeamChallenge.Logic
{
    public interface IProductBundleLogic
    {
        Task<IResponse> GetAllProductBundlesAsync();
        Task<IResponse> GetProductBundleByIdAsync(int id);
        //Task<IResponse> GetSortedProductAsync(int id);
        Task<IResponse> CreateProductBundleAsync(CreateProductBundleRequest dto);
        Task<IResponse> UpdateProductBundleAsync(int id, UpdateProductBundleRequest dto);
        Task<IResponse> DeleteProductBundleAsync(int id);
    }
}
