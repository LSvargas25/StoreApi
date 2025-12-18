using StoreApi.ModelsDTO.Item;

namespace StoreApi.Interface.Item
{
    public interface IItemService
    {
        //create Item 
        Task<ItemDTO> CreateAsync(ItemCreateDTO dto);
        //update Item
        Task<bool> UpdateAsync(int itemId, ItemUpdateDTO dto);
        //change status Item
        Task<bool> ChangeStatusAsync(int itemId, ItemChangeStatus dto);


        //get Item by id
        Task<ItemDTO?> GetByIdAsync(int itemId);

        //get all Items
        Task<List<ListItem>> GetAllAsync(string? search);

        //get all Items with attributes
        Task<List<ListItemWithAttribute>> GetAllWithAttributesAsync(string? search);

        //total delete  Item
        Task<bool> DeleteAsync(int itemId);

    

    }
}
