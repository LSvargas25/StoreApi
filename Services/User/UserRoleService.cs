using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using StoreApi.Interface.User;
using StoreApi.ModelsDTO.User;
using System.Data;

namespace StoreApi.Services.User
{

    public class UserRoleService : IUserRoleService
    {
        private readonly string _connectionString;

        public UserRoleService(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("StoreDb");
        }

        /// <summary>
        /// Retrieve a role by its ID.
        /// </summary>
        public async Task<RoleDTO> GetByIdAsync(int id)
        {
            RoleDTO? role = null;

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("[Users].[sp_Role_GetById]", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@RoleID", id);

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                role = new RoleDTO
                {
                    RoleId = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    IsActive = reader.GetBoolean(2)
                };
            }

            return role;
        }

        /// <summary>
        /// Get paginated roles with optional search.
        /// </summary>
        public async Task<List<RoleDTO>> GetAllAsync(string? search, int page, int limit)
        {
            var list = new List<RoleDTO>();

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("[Users].[sp_Role_GetAll]", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Search", (object?)search ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Page", page);
            cmd.Parameters.AddWithValue("@Limit", limit);

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                list.Add(new RoleDTO
                {
                    RoleId = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    IsActive = reader.GetBoolean(2)
                });
            }

            return list;
        }

        /// <summary>
        /// Create a new role and return its ID.
        /// </summary>
        public async Task<int> CreateAsync(RoleDTO dto)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("[Users].[sp_Role_Create]", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Name", dto.Name);
            cmd.Parameters.AddWithValue("@IsActive", dto.IsActive);

            var output = new SqlParameter("@NewRoleID", System.Data.SqlDbType.Int)
            {
                Direction = System.Data.ParameterDirection.Output
            };

            cmd.Parameters.Add(output);

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();

            return (int)output.Value;
        }

        public async Task<bool> UpdateAsync(int id, RoleDTO dto)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("[Users].[sp_Role_Update]", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@RoleID", id);
            cmd.Parameters.AddWithValue("@Name", dto.Name);
            cmd.Parameters.AddWithValue("@IsActive", dto.IsActive);

            await conn.OpenAsync();

            try
            {
                int rowsAffected = await cmd.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
            catch (SqlException ex) when (ex.Number == 50000) // RAISERROR
            {
                return false; // Role not found
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("[Users].[sp_Role_Delete]", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@RoleID", id);

            await conn.OpenAsync();

            try
            {
                int rowsAffected = await cmd.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
            catch (SqlException ex) when (ex.Number == 50000) // RAISERROR
            {
                return false; // Role not found
            }
        }

        public async Task<List<RoleName>> GetRoleNamesAsync()
        {
            var result = new List<RoleName>();

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("Users.sp_Role_GetNames", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                result.Add(new RoleName
                {
                    RoleId = reader.GetInt32(0),
                    Name = reader.GetString(1)
                });
            }

            return result;
        }
    }
}
