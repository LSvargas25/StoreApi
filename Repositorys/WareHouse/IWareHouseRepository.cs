using StoreApi.ModelsDTO.WareHouse;

namespace StoreApi.Repositorys.WareHouse
{
    public interface IWareHouseRepository
    {
        Task<int> CreateAsync(WareHouseCreateDTO dto);
        Task<bool> UpdateAsync(int id, WareHouseUpdateDTO dto);
        Task<List<WareHouseDTO>> GetAllAsync(string? search);
        Task<WareHouseDTO?> GetByIdAsync(int id);
        Task<bool> HardDeleteAsync(int id);
        Task<bool> ChangeStatusAsync(int id, bool isActive);

    }
}
