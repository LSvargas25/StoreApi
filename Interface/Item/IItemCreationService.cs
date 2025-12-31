using StoreApi.ModelsDTO.Item;
using static StoreApi.ModelsDTO.Item.ItemFullCreateDTO;

namespace StoreApi.Interface.Item
{
    public interface IItemCreationService
    {
        Task<ItemFullResponseDTO> CreateFullItemAsync(ItemFullCreateDTO dto);

    }

}
