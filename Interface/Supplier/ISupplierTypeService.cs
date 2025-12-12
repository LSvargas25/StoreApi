using StoreApi.ModelsDTO.Supplier;

namespace StoreApi.Interface.Supplier
{
    public interface ISupplierTypeService
    {
        //Create supplier type
        Task<int> CreateAsync(SupplierTypeDTO dto);
        
        //Get all the suppliertype
        Task<List<SupplierTypeDTO>> GetAllAsync(string? search, int page, int limit);
        //Delete supplier 
        Task<bool> DeleteAsync(int id);
        //update type 
        Task<bool> UpdateAsync(int id,SupplierTypeDTO dto);

    }
}

