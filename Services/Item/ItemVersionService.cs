using StoreApi.Interface.Item;
using StoreApi.ModelsDTO.Item;
using StoreApi.Repositorys.Item;

namespace StoreApi.Services.Item
{
    public class ItemVersionService : IItemVersionService
    {
        private readonly IItemVersionRepository _repository;

        public ItemVersionService(IItemVersionRepository repository)
        {
            _repository = repository;
        }

        public async Task CreateAsync(ItemVersionCreateDTO dto)
        {
            // 🔒 El servicio controla el versionado
            var lastVersion = await _repository.GetLastVersionNumberAsync(dto.ItemId);

            dto.VersionNumber = lastVersion + 1;
            dto.CreatedAt = DateTime.UtcNow;

            await _repository.CreateAsync(dto);
        }

        public Task<List<ItemVersionDTO>> GetByItemIdAsync(int itemId)
            => _repository.GetByItemIdAsync(itemId);
    }
}
