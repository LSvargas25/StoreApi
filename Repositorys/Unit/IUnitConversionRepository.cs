using StoreApi.ModelsDTO.Unit;

namespace StoreApi.Interface.Unit
{
    public interface IUnitConversionRepository
    {
        Task<int> CreateAsync(UnitConversionCreateDTO dto);
        Task<List<UnitConversionDTO>> GetAllAsync(string? search);
        Task<bool> UpdateAsync(int id, UnitConversionCreateDTO dto);
        Task<bool> DeleteAsync(int id);

        Task<decimal?> GetFactorAsync(int fromUnitId, int toUnitId);
        Task<List<UnitConversionDTO>> GetAllConversionsAsync();
        Task<string?> GetUnitNameAsync(int unitId);
    }
}
