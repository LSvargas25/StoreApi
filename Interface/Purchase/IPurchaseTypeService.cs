using StoreApi.ModelsDTO.Purshase;
using StoreApi.ModelsDTO.Supplier;

namespace StoreApi.Interface.Purchase
{
    public interface IPurchaseTypeService
    {
        // Returns a paginated list of types, optionally filtered by a search term.
        Task<List<PurchaseTypeDTO>> GetAllAsync(string? search, int page, int limit);

        // Creates a new Purshase Type and returns the ID of the newly created record.
        Task<int> CreateAsync(PurchaseTypeCreate dto);

        // Delete Purchase Type
        Task<bool> DeleteAsync(int id);

        //Update  a PurchaseType 
        Task<bool> UpdateAsync(int id ,PurchaseTypeUpdate dto);

        // Change the status for the purchase with the id
        Task<bool> ChangeStatusAsync(int id, PurchaseTypeStatus dto);

    }
}
