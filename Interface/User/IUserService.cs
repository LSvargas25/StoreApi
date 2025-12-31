using StoreApi.ModelsDTO.User;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StoreApi.Interface.User
{
    public interface IUserService
    {
        // crate a  user .
        Task<int> CreateAsync(UserAccountCreateDTO dto);
        // Update a  user by  ID.
        Task<bool> UpdateAsync(int id, UserUpdateDTO dto);

        Task<List<UserAccountDTO>> GetAllAsync(string? search);
        // Gets a  user by  ID.
        Task<UserAccountDTO?> GetByIdAsync(int id);
        // Delete a  user by  ID.
        Task<bool> DeleteAsync(int id);

        // Change the status for the UserID.
        Task<bool> ChangeStatus(int id, UserActiveDTO dto);

        // Change the Role for the UserID.
        Task<bool> ChangeRole(int id, UserRoleDTO dto);

        Task<bool> EditPhoto(int id, UserImageDTO dto);

    }
}