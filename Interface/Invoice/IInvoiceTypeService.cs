 
using StoreApi.ModelsDTO.Invoice;
 
namespace StoreApi.Interface.Invoice
{
    public interface IInvoiceTypeService
    {
        // Returns a paginated list of types, optionally filtered by a search term.
        Task<List<InvoiceTypeDTO>> GetAllAsync(string? search, int page, int limit);

        // Creates a new invoice type and returns the ID of the newly created record.
        Task<int> CreateAsync(InvoiceTypeCretDTO dto);

        // Delete a inovice type
        Task<bool> DeleteAsync(int id);

        //Update a Invoice Type
        Task<bool> UpdateAsync(int id, InvoiceTypeCretDTO dto);

        //Change status
        Task<bool> ChangeStatusAsync(int id, InvoiceChangeStatus dto);

    }
}
