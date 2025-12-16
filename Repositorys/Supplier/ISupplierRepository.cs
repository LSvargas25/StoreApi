using StoreApi.ModelsDTO.Supplier;

namespace StoreApi.Repositorys.Supplier
{
    public interface ISupplierRepository
    {
        Task<int> CreateAsync(CreateSupplier dto);
        Task<bool> UpdateAsync(int id, SupplierUpdate dto);
        Task<List<SupplierSee>> GetAllForViewAsync(string? search);
        Task<SupplierDTO?> GetByIdAsync(int id);
        Task<bool> DeleteAsync(int id);
        Task<bool> ChangeStatusAsync(int id, bool isActive);
        Task<bool> ChangeRoleAsync(int id, int? supplierTypeId);

    }
}
