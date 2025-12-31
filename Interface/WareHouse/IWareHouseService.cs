using StoreApi.ModelsDTO.WareHouse;

namespace StoreApi.Interface.WareHouse
{
    public interface IWareHouseService
    {
        Task<int> CreateAsync(WareHouseCreateDTO dto);
        Task<bool> UpdateAsync(int id, WareHouseUpdateDTO dto);
        Task<List<WareHouseDTO>> GetAllAsync(string? search);
        Task<WareHouseDTO?> GetByIdAsync(int id);
        Task<bool> ChangeStatusAsync(int id, bool isActive);
        Task<HardDeleteResultDTO> HardDeleteAsync(int warehouseId, int userId);
    }
}
