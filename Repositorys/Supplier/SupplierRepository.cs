using Microsoft.Data.SqlClient;
using StoreApi.Interface.Supplier;
using StoreApi.ModelsDTO.Supplier;
using StoreApi.Repositorys.Supplier;
using StoreApi.Tools;
using System.Data;

namespace StoreApi.Repository.Supplier
{
    public class SupplierRepository : ISupplierRepository
    {
        private readonly string _connectionString;


        public SupplierRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("StoreDb");
        }



        public async Task<int> CreateAsync(CreateSupplier dto)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("Supplier.sp_Supplier_Create", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Name", dto.Name);
            cmd.Parameters.AddWithValue("@Email", (object?)dto.Email ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@PhoneNumber", (object?)dto.PhoneNumber ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@SupplierTypeID", dto.SupplierTypeId ?? (object)DBNull.Value);

            var output = new SqlParameter("@NewID", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(output);

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();

            return (int)output.Value;
        }

        public async Task<List<SupplierSee>> GetAllForViewAsync(string? search)
        {
            var list = new List<SupplierSee>();

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("Supplier.sp_Supplier_GetAll", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Search", (object?)search ?? DBNull.Value);

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                list.Add(new SupplierSee
                {
                    SupplierId = reader.GetInt32(reader.GetOrdinal("SupplierID")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),

                    PhoneNumber = reader.IsDBNull(reader.GetOrdinal("PhoneNumber"))
                        ? null
                        : reader.GetString(reader.GetOrdinal("PhoneNumber")),

                    IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),

                    SupplierTypeName = reader.IsDBNull(reader.GetOrdinal("SupplierTypeName"))
                        ? null
                        : reader.GetString(reader.GetOrdinal("SupplierTypeName"))
                });
            }


            return list;
        }


        public async Task<SupplierDTO?> GetByIdAsync(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("Supplier.sp_Supplier_GetById", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@SupplierID", id);

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
                return MapSupplier(reader);

            return null;
        }

        public async Task<bool> UpdateAsync(int id, SupplierUpdate dto)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("Supplier.sp_Supplier_Update", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@SupplierID", id);
            cmd.Parameters.AddWithValue("@Name", dto.Name);
            cmd.Parameters.AddWithValue("@Email", (object?)dto.Email ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@PhoneNumber", (object?)dto.PhoneNumber ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@SupplierTypeID", dto.SupplierTypeId);

            await conn.OpenAsync();
            var result = await cmd.ExecuteScalarAsync();

            return Convert.ToInt32(result ?? 0) > 0;
        }


        public async Task<bool> DeleteAsync(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("Supplier.sp_Supplier_Delete", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@SupplierID", id);

            await conn.OpenAsync();
            var result = await cmd.ExecuteScalarAsync();

            return Convert.ToInt32(result ?? 0) > 0;
        }

        public async Task<bool> ChangeStatusAsync(int id, bool isActive)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("Supplier.sp_Supplier_ChangeStatus", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@SupplierID", id);
            cmd.Parameters.AddWithValue("@IsActive", isActive);

            await conn.OpenAsync();
            var result = await cmd.ExecuteScalarAsync();

            return Convert.ToInt32(result ?? 0) > 0;
        }

        public async Task<bool> ChangeRoleAsync(int id, int? supplierTypeId)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("Supplier.sp_Supplier_ChangeRole", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@SupplierID", id);
            cmd.Parameters.AddWithValue("@SupplierTypeID", supplierTypeId ?? (object)DBNull.Value);

            await conn.OpenAsync();
            var result = await cmd.ExecuteScalarAsync();

            return Convert.ToInt32(result ?? 0) > 0;
        }

        private SupplierDTO MapSupplier(SqlDataReader reader)
        {
            int idxId = reader.GetOrdinal("SupplierID");
            int idxName = reader.GetOrdinal("Name");
            int idxEmail = reader.GetOrdinal("Email");
            int idxPhone = reader.GetOrdinal("PhoneNumber");
            int idxIsActive = reader.GetOrdinal("IsActive");
            int idxType = reader.GetOrdinal("SupplierTypeID");
            int idxCreated = reader.GetOrdinal("CreatedAt");
            int idxUpdated = reader.GetOrdinal("UpdatedAt");

            var encName = reader.GetString(idxName);
            var encEmail = reader.IsDBNull(idxEmail) ? null : reader.GetString(idxEmail);
            var encPhone = reader.IsDBNull(idxPhone) ? null : reader.GetString(idxPhone);

            return new SupplierDTO
            {
                SupplierId = reader.GetInt32(idxId),
                Name = reader.GetString(idxName),
                Email = reader.IsDBNull(idxEmail) ? null : reader.GetString(idxEmail),
                PhoneNumber = reader.IsDBNull(idxPhone) ? null : reader.GetString(idxPhone),
                IsActive = reader.GetBoolean(idxIsActive),
                SupplierTypeId = reader.IsDBNull(idxType) ? null : reader.GetInt32(idxType),

                CreatedAt = reader.IsDBNull(idxCreated)
         ? null
         : DateOnly.FromDateTime(reader.GetDateTime(idxCreated)),

                UpdatedAt = reader.IsDBNull(idxUpdated)
         ? null
         : DateOnly.FromDateTime(reader.GetDateTime(idxUpdated))
            };

        }


    }
}
