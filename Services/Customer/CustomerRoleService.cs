using Microsoft.Data.SqlClient;
using StoreApi.ModelsDTO.Customer;
using StoreApi.Interface.Customer;
using System.Data;

namespace StoreApi.Services.Customer
{
    public class CustomerRoleService:ICustomerRoleService
    {
        private readonly string _connectionString;
        public CustomerRoleService(IConfiguration config) {
            _connectionString = config.GetConnectionString("StoreDb");
        }

        //GET ALL THE CUSTOMER TYPE 
        public async Task<List<CustomerRoleDTO>> GetAllAsync(string? search, int page, int limit)
        {
            var list = new List<CustomerRoleDTO>();

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("[Customer].[sp_CustomerType_GetAll]", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Search", (object?)search ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Page", page);
            cmd.Parameters.AddWithValue("@Limit", limit);

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                list.Add(new CustomerRoleDTO
                {
                    CustomerRoleId = reader.GetInt32(0),
                    Name = reader.GetString(1),
                });
            }

            return list;
        }

        //CREATE A NEW CUSTOMER TYPE 
        public async Task<int> CreateAsync(CustomerRoleCreat dto)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("[Customer].[sp_CustomerType_Create]", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Name", dto.Name);

            var output = new SqlParameter("@NewCustomerTypeID", System.Data.SqlDbType.Int)
            {
                Direction = System.Data.ParameterDirection.Output
            };

            cmd.Parameters.Add(output);

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();

            return (int)output.Value;
        }

        //DELETE CUSTOMER  TYPE
        public async Task<bool> DeleteAsync(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("[Customer].[sp_CustomerType_Delete]", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@CustomerTypeID", id);

            await conn.OpenAsync();
            var rows = await cmd.ExecuteScalarAsync();

            return (int)(rows ?? 0) > 0;
        }
        //Update customer
        public async Task<bool> UpdateAsync(int id, CustomerRoleUpdt dto)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("[Customer].[sp_CustomerType_Update]", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@CustomerRoleID", id);
            cmd.Parameters.AddWithValue("@Name", dto.Name);

            await conn.OpenAsync();

            var result = await cmd.ExecuteScalarAsync();
            return Convert.ToInt32(result) > 0;
        }

        public async Task<bool> ChangeStatusAsync(int id, CustomerRoleChangs dto)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("[Customer].[sp_CustomerType_ChangeStatus]", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@CustomerRoleID", id);
            cmd.Parameters.AddWithValue("@IsActive", dto.IsActive);

            await conn.OpenAsync();

            var result = await cmd.ExecuteScalarAsync();
            return Convert.ToInt32(result) > 0;
        }


    }
}
