using StoreApi.Interface.WareHouse;
using StoreApi.ModelsDTO.WareHouse;
using StoreApi.Repositorys.WareHouse;

namespace StoreApi.Services.WareHouse
{
    public class WareHouseService : IWareHouseService
    {
        private readonly IWareHouseRepository _repo;

        public WareHouseService(IWareHouseRepository repo)
        {
            _repo = repo;
        }

        public Task<int> CreateAsync(WareHouseCreateDTO dto) => _repo.CreateAsync(dto);
        public Task<bool> UpdateAsync(int id, WareHouseUpdateDTO dto) => _repo.UpdateAsync(id, dto);
        public Task<List<WareHouseDTO>> GetAllAsync(string? search) => _repo.GetAllAsync(search);
        public Task<WareHouseDTO?> GetByIdAsync(int id) => _repo.GetByIdAsync(id);
        public Task<bool> ChangeStatusAsync(int id, bool isActive) => _repo.ChangeStatusAsync(id, isActive);
        public Task<bool> HardDeleteAsync(int id) => _repo.HardDeleteAsync(id);
    }
}
