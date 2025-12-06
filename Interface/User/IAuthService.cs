using StoreApi.ModelsDTO.User;
using System.Threading.Tasks;
namespace StoreApi.Interface.User

{
    public interface IAuthService
    {
        Task<UserLoginResponseDTO?> LoginAsync(UserLoginRequestDTO request);
    }
}
