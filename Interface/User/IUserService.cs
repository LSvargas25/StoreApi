using StoreApi.ModelsDTO.User;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StoreApi.Interface.User
{
    // This interface defines the contract for a user service,
    // specifying the operations related to user management and authentication.
    public interface IUserService
    {
        // POST /auth/login
        // Authenticates a user based on the provided login credentials (DTO).
        // Returns a response DTO containing authentication details (e.g., token).
        Task<UserLoginResponseDTO> LoginAsync(UserLoginRequestDTO dto);

        // POST /users
        // Creates a new user account with the details provided in the DTO.
        // Returns the ID of the newly created user account.
        Task<int> CreateAsync(UserAccountDTO dto);

        // GET /users
        // Retrieves a list of all user accounts.
        // Allows optional searching (by 'search' string) and pagination (by 'page' and 'limit').
        Task<List<UserAccountDTO>> GetAllAsync(string? search, int page, int limit);

        // GET /users/{id}
        // Retrieves a single user account by its unique identifier (id).
        Task<UserAccountDTO> GetByIdAsync(int id);

        // PUT /users/{id}
        // Updates the details of an existing user account specified by its id,
        // using the data provided in the DTO.
        // Returns true if the update was successful, false otherwise.
        Task<bool> UpdateAsync(int id, UserAccountDTO dto);

        // DELETE /users/{id}
        // Deletes the user account specified by its unique identifier (id).
        // Returns true if the deletion was successful, false otherwise.
        Task<bool> DeleteAsync(int id);
    }
}