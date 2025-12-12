using StoreApi.ModelsDTO.Customer;
using StoreApi.ModelsDTO.Item;

namespace StoreApi.Interface.Customer
{
    public interface ICustomerRoleService
    {

        // Returns a paginated list of roles, optionally filtered by a search term.
        Task<List<CustomerRoleDTO>> GetAllAsync(string? search, int page, int limit);

        // Creates a new role and returns the ID of the newly created record.
        Task<int> CreateAsync(CustomerRoleCreat dto);

        // Delete Role
        Task<bool> DeleteAsync(int id);

        // Update a  Customer type  by  ID
        Task<bool> UpdateAsync(int id,CustomerRoleUpdt dto);

        //Change status
        Task<bool> ChangeStatusAsync(int id, CustomerRoleChangs dto);

    }
}
