using Microsoft.Data.SqlClient;
using StoreApi.Interface.Invoice;
using StoreApi.ModelsDTO.Customer;
using StoreApi.ModelsDTO.Invoice;
using System.Data;

namespace StoreApi.Services.Invoice
{
    public class InvoiceTypeService:IInvoiceTypeService
    {
        private readonly string _connectionString;
        public InvoiceTypeService(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("StoreDb");
        }

        public async Task<bool> ChangeStatusAsync(int id, InvoiceChangeStatus dto)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("[Invoice].[sp_InvoiceType_ChangeStatus]", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@InvoiceTypeID", id);
            cmd.Parameters.AddWithValue("@IsActive", dto.IsActive);

            await conn.OpenAsync();

            var result = await cmd.ExecuteScalarAsync();
            return Convert.ToInt32(result) > 0;
        }

        public async Task<int> CreateAsync(InvoiceTypeCretDTO dto)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("[Invoice].[sp_InvoiceType_Create]", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Name", dto.Name);

            var output = new SqlParameter("@NewInvoiceTypeID", System.Data.SqlDbType.Int)
            {
                Direction = System.Data.ParameterDirection.Output
            };

            cmd.Parameters.Add(output);

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();

            return (int)output.Value;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("[Invoice].[sp_InvoiceType_Delete]", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@InvoiceTypeID", id);

            await conn.OpenAsync();
            var rows = await cmd.ExecuteScalarAsync();

            return (int)(rows ?? 0) > 0;
        }

        public async Task<List<InvoiceTypeDTO>> GetAllAsync(string? search, int page, int limit)
        {
            var list = new List<InvoiceTypeDTO>();

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("[Invoice].[sp_InvoiceType_GetAll]", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Search", (object?)search ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Page", page);
            cmd.Parameters.AddWithValue("@Limit", limit);

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                list.Add(new InvoiceTypeDTO
                {
                    InvoiceTypeId = reader.GetInt32(reader.GetOrdinal("InvoiceTypeID")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
                });
            }

            return list;
        }



        public async Task<bool> UpdateAsync(int id, InvoiceTypeCretDTO dto)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("[Invoice].[sp_InvoiceType_Update]", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@InvoiceTypeID", id);
            cmd.Parameters.AddWithValue("@Name", dto.Name);

            await conn.OpenAsync();

            var result = await cmd.ExecuteScalarAsync();
            return Convert.ToInt32(result) > 0;
        }

    }
}
