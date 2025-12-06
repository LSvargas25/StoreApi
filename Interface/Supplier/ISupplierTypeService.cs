using StoreApi.ModelsDTO.Supplier;

namespace StoreApi.Interface.Supplier
{
    public interface ISupplierTypeService
    {

        // crate a  role Supplier .
        Task<int> CreateAsync(SupplierTypeDTO dto);
        
        // Gets all  types at bd.
        Task<List<SupplierDTO>> GetAllAsync(string? search);
      
        // Delete a  supplier by  ID.
        Task<bool> DeleteAsync(int id);
    }
}

