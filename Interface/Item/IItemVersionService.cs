using StoreApi.ModelsDTO.Item;

namespace StoreApi.Interface.Item
{
    public interface IItemVersionService
    {
        Task CreateAsync(ItemVersionCreateDTO dto);
        Task<List<ItemVersionDTO>> GetByItemIdAsync(int itemId);
    }
}
