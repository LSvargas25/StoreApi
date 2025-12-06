using StoreApi.Models;
using StoreApi.ModelsDTO.User;
using System.Threading.Tasks;
namespace StoreApi.Repositorys.User
{
    public interface IUserRepository
    {
        Task<UserAccount?> GetByEmailAsync(string email);
        Task<int> CreateAsync(UserAccountCreateDTO dto, byte[] hash, byte[] salt);
    }
}
