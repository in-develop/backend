using TeamChallenge.Models.DTOs.Product;
using TeamChallenge.Models.Entities;


namespace TeamChallenge.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductEntity>> GetAllProductsAsync();
        Task<ProductEntity?> GetProductByIdAsync(int id);
        Task<ProductEntity> CreateProductAsync(ProductCreateDto dto);
        Task<bool> UpdateProductAsync(int id, ProductUpdateDto dto);
        Task<bool> DeleteProductAsync(int id);
    }
}
