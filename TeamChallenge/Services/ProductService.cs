using TeamChallenge.Models.Entities;
using TeamChallenge.Interfaces;
using TeamChallenge.Models.DTOs.Product;
using TeamChallenge.Repositories;

namespace TeamChallenge.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(RepositoryFactory factory)
        {
            _productRepository = (IProductRepository)factory.GetRepository<ProductEntity>();
        }

        public async Task<IEnumerable<ProductEntity>> GetAllProductsAsync()
        {
            return await _productRepository.GetAllAsync();
        }

        public async Task<ProductEntity?> GetProductByIdAsync(int id)
        {
            return await _productRepository.GetByIdAsync(id);
        }

        public async Task<ProductEntity> CreateProductAsync(ProductCreateDto dto)
        {
            var product = new ProductEntity
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                CategoryId = dto.CategoryId,
            };
            //await _productRepository.CreateAsync(product);
            return product;
        }
        public async Task<bool> UpdateProductAsync(int id, ProductUpdateDto dto)
        {
            var product = new ProductEntity
            {
                Id = id,
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                CategoryId = dto.CategoryId,
            };
            /*if (await _productRepository.UpdateAsync(id, product))
            {
                return true;
            }*/
            return false;
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var deleted = await _productRepository.DeleteAsync(id);
            if (!deleted)
            {
                throw new KeyNotFoundException($"Product with Id={id} not found");
            }
            return true;
        }




















        //public class ProductService : IProductService
        //{
        //private readonly CosmeticStoreDbContext _context;

        //    public CosmeticService(CosmeticStoreDbContext context)
        //    {
        //        _context = context;
        //    }

        //public async Task<List<CosmeticReadDto>> GetAllAsync()
        //{
        //    return await _context.Cosmetiс
        //        .AsNoTracking()
        //        .Select(c => new CosmeticReadDto
        //        {
        //            Id = c.Id,
        //            Name = c.Name,
        //            Description = c.Description,
        //            Price = c.Price,
        //            Category = c.Category
        //        }).ToListAsync();
        //}

        //public async Task<CosmeticReadDto?> GetByIdAsync(int id)
        //{
        //    return await _context.Cosmetiс
        //        .AsNoTracking()
        //        .Where(c => c.Id == id)
        //        .Select(c => new CosmeticReadDto
        //        {
        //            Id = c.Id,
        //            Name = c.Name,
        //            Description = c.Description,
        //            Price = c.Price,
        //            Category = c.Category
        //        }).FirstOrDefaultAsync(); // CosmeticObject bug
        //}

        //public async Task<CosmeticReadDto> CreateAsync(CosmeticCreateDto dto)
        //{
        //    var cosmetic = new Cosmetic
        //    {
        //        Name = dto.Name,
        //        Description = dto.Description,
        //        Price = dto.Price,
        //        Category = await _context.Category
        //        .Where(cat => dto.CategoryIds.Contains(cat.Id))
        //        .ToListAsync()
        //    };

        //    _context.Cosmetiс.Add(cosmetic);
        //    await _context.SaveChangesAsync();

        //    return new CosmeticReadDto
        //    {
        //        Id = cosmetic.Id,
        //        Name = cosmetic.Name,
        //        Description = cosmetic.Description,
        //        Price = cosmetic.Price,
        //        Category = cosmetic.Category
        //    };
        //}

        //public async Task<bool> UpdateAsync(int id, CosmeticCreateDto dto)
        //{
        //    var cosmetic = await _context.Cosmetiс
        //        .FirstOrDefaultAsync(c => c.Id == id);

        //    if (cosmetic == null) 
        //    {
        //        return false;
        //    }

        //    cosmetic.Name = dto.Name;
        //    cosmetic.Description = dto.Description;
        //    cosmetic.Price = dto.Price;
        //    cosmetic.Category = await _context.Category
        //        .Where(cat => dto.CategoryIds.Contains(cat.Id))
        //        .ToListAsync();

        //    await _context.SaveChangesAsync();
        //    return true;
        //}

        //    public async Task<bool> DeleteAsync(int id)
        //    {
        //        var cosmetic = await _context.Cosmetiс.FindAsync(id);
        //        if (cosmetic == null) 
        //        {
        //            return false;
        //        }

        //        _context.Cosmetiс.Remove(cosmetic);
        //        await _context.SaveChangesAsync();
        //        return true;
        //    }
        //}
    }
}
