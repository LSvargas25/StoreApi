using StoreApi.ModelsDTO.Item;
using StoreApi.ModelsDTO.Purshase;

namespace StoreApi.Interface.Item
{
    public interface IItemCategoryService
    {
        // Returns a paginated list of types, optionally filtered by a search term.
        Task<List<ItemCategoryDTO>> GetAllAsync(string? search, int page, int limit);

        // Creates a new Purshase Type and returns the ID of the newly created record.
        Task<int> CreateAsync(ItemCatCreate dto);

        // Delete Purshase Type
        Task<bool> DeleteAsync(int id);

        //Update Item Category
        Task<bool> UpdateAsync(int id, ItemCatUpdt dto);


        // Change the status for the Item Category with the id
        Task<bool> ChangeStatusAsync(int id, ItemChangStatus dto);
    }
}
