using Microsoft.Data.SqlClient;
using StoreApi.Models;
using StoreApi.ModelsDTO.User;
using System.Data;

namespace StoreApi.Repositorys.User
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;
        public UserRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("StoreDb");
        }

        public async Task<UserAccount?> GetByEmailAsync(string email)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("Auth.sp_Login_GetUserByEmail", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@Email", email);

            await conn.OpenAsync();
            using var rdr = await cmd.ExecuteReaderAsync();

            if (!await rdr.ReadAsync())
                return null;

            return new UserAccount
            {
                UserId = rdr.GetInt32("UserID"),
                UserName = rdr.GetString("UserName"),
                Email = rdr.GetString("Email"),
                PasswordHash = (byte[])rdr["PasswordHash"],
                PasswordSalt = (byte[])rdr["PasswordSalt"],
                IsActive = rdr.GetBoolean("IsActive"),
                RoleId = rdr.GetInt32("RoleID"),
                CreatedAt = rdr.IsDBNull("CreatedAt") ? null : rdr.GetDateTime("CreatedAt"),
                UpdatedAt = rdr.IsDBNull("UpdatedAt") ? null : rdr.GetDateTime("UpdatedAt")
            };
        }

    }
}