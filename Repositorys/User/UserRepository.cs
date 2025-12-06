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
            using var cmd = new SqlCommand("Users.sp_User_GetByEmail", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@Email", email);

            await conn.OpenAsync();
            using var rdr = await cmd.ExecuteReaderAsync();
            if (!rdr.HasRows) return null;

            if (await rdr.ReadAsync())
            {
                return new UserAccount
                {
                    UserId = rdr.GetInt32(rdr.GetOrdinal("UserID")),
                    UserName = rdr.GetString(rdr.GetOrdinal("UserName")),
                    Email = rdr.GetString(rdr.GetOrdinal("Email")),
                    PasswordHash = (byte[])rdr["PasswordHash"],
                    PasswordSalt = (byte[])rdr["PasswordSalt"],
                    PhoneNumber = rdr.IsDBNull(rdr.GetOrdinal("PhoneNumber")) ? null : rdr.GetString(rdr.GetOrdinal("PhoneNumber")),
                    CardId = rdr.IsDBNull(rdr.GetOrdinal("CardID")) ? null : rdr.GetString(rdr.GetOrdinal("CardID")),
                    IsActive = rdr.GetBoolean(rdr.GetOrdinal("IsActive")),
                    RoleId = rdr.GetInt32(rdr.GetOrdinal("RoleID")),
                    CreatedAt = rdr.GetDateTime(rdr.GetOrdinal("CreatedAt")),
                    UpdatedAt = rdr.IsDBNull(rdr.GetOrdinal("UpdatedAt")) ? DateTime.MinValue : rdr.GetDateTime(rdr.GetOrdinal("UpdatedAt"))
                };
            }

            return null;
        }

        public async Task<int> CreateAsync(UserAccountCreateDTO dto, byte[] hash, byte[] salt)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("Users.sp_User_Create", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@UserName", dto.UserName);
            cmd.Parameters.AddWithValue("@Email", dto.Email);
            cmd.Parameters.Add("@PasswordHash", SqlDbType.VarBinary, 512).Value = hash;
            cmd.Parameters.Add("@PasswordSalt", SqlDbType.VarBinary, 128).Value = salt;
            cmd.Parameters.AddWithValue("@PhoneNumber", (object?)dto.PhoneNumber ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CardID", (object?)dto.CardId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@IsActive", true);
            cmd.Parameters.AddWithValue("@RoleID", dto.RoleId);
            cmd.Parameters.AddWithValue("@CreatedAt", DateTime.UtcNow);
            cmd.Parameters.AddWithValue("@UpdatedAt", DBNull.Value);

            var outParam = new SqlParameter("@NewUserID", SqlDbType.Int) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(outParam);

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();

            return (int)outParam.Value!;
        }
    }
}