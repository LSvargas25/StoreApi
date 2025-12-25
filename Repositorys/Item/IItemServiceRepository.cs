using StoreApi.ModelsDTO.Item;

namespace StoreApi.Repositorys.Item
{
    public interface IItemVersionRepository
    {
        Task CreateAsync(ItemVersionCreateDTO dto);
        Task<List<ItemVersionDTO>> GetByItemIdAsync(int itemId);

        // 🔒 Uso interno del servicio
        Task<int> GetLastVersionNumberAsync(int itemId);
    }
}
