using StoreApi.ModelsDTO;
using StoreApi.ModelsDTO.User;

namespace StoreApi.Interface.User
{
    public interface IUserRoleService
    {
        // Gets a single role by its ID.
        Task<RoleDTO> GetByIdAsync(int id);

        // Returns a paginated list of roles, optionally filtered by a search term.
        Task<List<RoleDTO>> GetAllAsync(string? search, int page, int limit);

        // Creates a new role and returns the ID of the newly created record.
        Task<int> CreateAsync(RoleDTO dto);

        // Updates an existing role identified by its ID. Returns true if the update was successful.
        Task<bool> UpdateAsync(int id, RoleDTO dto);

        // Deletes a role by its ID. Returns true if the deletion was successful.
        Task<bool> DeleteAsync(int id);
    }
}
