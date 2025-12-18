using StoreApi.Interface.Item;
using StoreApi.ModelsDTO.Item;
using StoreApi.Repositorys.Item;

namespace StoreApi.Services.Item
{
    public class ItemService : IItemService
    {
        private readonly IItemRepository _repo;

        public ItemService(IItemRepository repo)
        {
            _repo = repo;
        }

        public async Task<ItemDTO> CreateAsync(ItemCreateDTO dto)
        {
            var newId = await _repo.CreateAsync(dto);
            return (await _repo.GetByIdAsync(newId))!;
        }

        public Task<bool> UpdateAsync(int itemId, ItemUpdateDTO dto)
            => _repo.UpdateAsync(itemId, dto);

        public Task<bool> ChangeStatusAsync(int itemId, ItemChangeStatus dto)
            => _repo.ChangeStatusAsync(itemId, dto.IsActive);

        public Task<ItemDTO?> GetByIdAsync(int itemId)
            => _repo.GetByIdAsync(itemId);

        public Task<List<ListItem>> GetAllAsync(string? search)
            => _repo.GetAllAsync(search);

        public Task<List<ListItemWithAttribute>> GetAllWithAttributesAsync(string? search)
            => _repo.GetAllWithAttributesAsync(search);

        public Task<bool> DeleteAsync(int itemId)
            => _repo.DeleteAsync(itemId);

       
    }
}
