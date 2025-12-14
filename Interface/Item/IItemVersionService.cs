
using StoreApi.ModelsDTO.Item;

namespace StoreApi.Interface.Item
{
    public interface IItemVersionService
    {
        //create a new version
        Task CreateAsync(ItemVersionCreateDTO dto);

        //list all the version 
        Task<List<ItemVersionDTO>> GetByItemIdAsync(int itemId);

    }
}
