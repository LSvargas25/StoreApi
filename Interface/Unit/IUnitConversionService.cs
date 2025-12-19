using StoreApi.ModelsDTO.Unit;
using StoreApi.ModelsDTO.Unit.StoreApi.ModelsDTO.Unit;

namespace StoreApi.Interface.Unit
{
    public interface IUnitConversionService
    {
        Task<UnitConversionCreateDTO> CreateUnitConversionAsync(UnitConversionCreateDTO dto);
        Task<List<UnitConversionDTO>> GetAllUnitConversionsAsync(string? search);
        Task<bool> UpdateUnitConversionAsync(int id, UnitConversionCreateDTO dto);
        Task<bool> DeleteUnitConversionAsync(int id);

        Task<UnitConversionResultDTO> ConvertAsync(
            int fromUnitId,
            int toUnitId,
            decimal value);
    }
}
