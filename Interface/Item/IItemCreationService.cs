using StoreApi.ModelsDTO.Item;

namespace StoreApi.Interface.Item
{
    public interface IItemCreationService
    {
        Task<int> CreateFullItemAsync(ItemFullCreateDTO dto);
    }

}
