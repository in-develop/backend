

namespace TeamChallenge.Interfaces
{
    public interface ICosmetic
    {
        Task<List<CosmeticReadDto>> GetAllAsync();
        Task<CosmeticReadDto?> GetByIdAsync(int id);
        Task<CosmeticReadDto> CreateAsync(CosmeticCreateDto dto);
        Task<bool> UpdateAsync(int id, CosmeticAddDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
