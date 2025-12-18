using StoreApi.ModelsDTO.Item;

namespace StoreApi.Repositorys.Item
{
    public interface IItemRepository
    {
        Task<int> CreateAsync(ItemCreateDTO dto);

        Task<bool> UpdateAsync(int itemId, ItemUpdateDTO dto);

        Task<bool> ChangeStatusAsync(int itemId, bool isActive);

        Task<ItemDTO?> GetByIdAsync(int itemId);

        Task<List<ListItem>> GetAllAsync(string? search);
        

        Task<List<ListItemWithAttribute>> GetAllWithAttributesAsync(string? search);

        Task<bool> DeleteAsync(int itemId);
    }
}
