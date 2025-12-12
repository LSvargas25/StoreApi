 
using StoreApi.ModelsDTO.Tax;

namespace StoreApi.Interface.Tax
{
    public interface ITaxService
    {
        // Returns a paginated list of tax, optionally filtered by a search term.
        Task<List<TaxDTO>> GetAllAsync(string? search, int page, int limit);

        // Creates a new tax and returns the ID of the newly created record.
        Task<int> CreateAsync(TaxDTO dto);

        // Delete a tax
        Task<bool> DeleteAsync(int id);

        // Update a  tax by  ID
        Task<bool> UpdateAsync(int id, TaxDTO dto);
    }
}
