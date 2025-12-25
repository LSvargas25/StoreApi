using System.Text.Json;
using StoreApi.Interface.Item;
using StoreApi.ModelsDTO.Item;
using StoreApi.Repositorys.Item;

namespace StoreApi.Services.Item
{
    public class ItemService : IItemService
    {
        private readonly IItemRepository _repo;
        private readonly IItemVersionService _itemVersionService;

        public ItemService(
            IItemRepository repo,
            IItemVersionService itemVersionService)
        {
            _repo = repo;
            _itemVersionService = itemVersionService;
        }

        public async Task<ItemDTO> CreateAsync(ItemCreateDTO dto)
        {
            var newId = await _repo.CreateAsync(dto);
            var item = (await _repo.GetByIdAsync(newId))!;

            await _itemVersionService.CreateAsync(new ItemVersionCreateDTO
            {
                ItemId = item.ItemId,
                DataSnapshot = JsonSerializer.Serialize(item)
            });

            return item;
        }

        public async Task<bool> UpdateAsync(int itemId, ItemUpdateDTO dto)
        {
            var updated = await _repo.UpdateAsync(itemId, dto);
            if (!updated) return false;

            var item = await _repo.GetByIdAsync(itemId);
            if (item == null) return false;

            await _itemVersionService.CreateAsync(new ItemVersionCreateDTO
            {
                ItemId = itemId,
                DataSnapshot = JsonSerializer.Serialize(item)
            });

            return true;
        }

        public async Task<bool> ChangeStatusAsync(int itemId, ItemChangeStatus dto)
        {
            var changed = await _repo.ChangeStatusAsync(itemId, dto.IsActive);
            if (!changed) return false;

            var item = await _repo.GetByIdAsync(itemId);
            if (item == null) return false;

            await _itemVersionService.CreateAsync(new ItemVersionCreateDTO
            {
                ItemId = itemId,
                DataSnapshot = JsonSerializer.Serialize(item)
            });

            return true;
        }

        public async Task<bool> DeleteAsync(int itemId)
        {
            var item = await _repo.GetByIdAsync(itemId);
            if (item == null) return false;

            await _itemVersionService.CreateAsync(new ItemVersionCreateDTO
            {
                ItemId = itemId,
                DataSnapshot = JsonSerializer.Serialize(item)
            });

            return await _repo.DeleteAsync(itemId);
        }

        public Task<ItemDTO?> GetByIdAsync(int itemId)
            => _repo.GetByIdAsync(itemId);

        public Task<List<ListItem>> GetAllAsync(string? search)
            => _repo.GetAllAsync(search);

        public Task<List<ListItemWithAttribute>> GetAllWithAttributesAsync(string? search)
            => _repo.GetAllWithAttributesAsync(search);
    }
}
