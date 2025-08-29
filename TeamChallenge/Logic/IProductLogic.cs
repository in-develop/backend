using TeamChallenge.Models.Requests;
using TeamChallenge.Models.Responses;


namespace TeamChallenge.Logic
{
    public interface IProductLogic
    {
        Task<IResponse> GetAllProductsAsync();
        Task<IResponse> GetProductByIdAsync(int id);
        //Task<IResponse> GetSortedProductAsync(int id);
        Task<IResponse> CreateProductAsync(CreateProductRequest dto);
        Task<IResponse> UpdateProductAsync(int id, UpdateProductRequest dto);
        Task<IResponse> DeleteProductAsync(int id);
    }
}
