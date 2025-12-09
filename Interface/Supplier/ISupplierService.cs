using StoreApi.ModelsDTO.Supplier;

namespace StoreApi.Interface.Supplier
{
    public interface ISupplierService
    {

        // crate a  Supplier .
        Task<int> CreateAsync(CreateSupplier dto);
        // Update a  supplier by  ID.
        Task<bool> UpdateAsync(int id, SupplierUpdate dto);
        // Gets all  suppliers at bd.
        Task<List<SupplierDTO>> GetAllAsync(string? search);
        // Gets a  supplier by  ID.
        Task<SupplierDTO?> GetByIdAsync(int id);
        // Delete a  supplier by  ID.
        Task<bool> DeleteAsync(int id);

        // Change the status for supplier with the id
        Task<bool> ChangeStatus(int id, SupplierStatus dto);

        // Change the Role for the supplier for id 
        Task<bool> ChangeRole(int id, SupplierRole dto);
    }
}
