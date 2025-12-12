using Microsoft.Data.SqlClient;
using StoreApi.Interface.Purchase;
using StoreApi.ModelsDTO.Purshase;
using System.Data;

namespace StoreApi.Services.Purchase
{
    public class PurchaseTypeService : IPurchaseTypeService
    {
        private readonly string _connectionString;

        public PurchaseTypeService(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("StoreDb");
        }

        //CREATE Purchase Type
        public async Task<int> CreateAsync(PurchaseTypeCreate dto)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("[Purchase].[sp_PurchaseType_Create]", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Name", dto.Name);

            var output = new SqlParameter("@NewPurchaseTypeID", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(output);

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();

            return (int)output.Value;
        }

        //DELETE Purchase Type
        public async Task<bool> DeleteAsync(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("[Purchase].[sp_PurchaseType_Delete]", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@PurchaseTypeID", id);

            await conn.OpenAsync();
            var rows = await cmd.ExecuteScalarAsync();

            return (int)(rows ?? 0) > 0;
        }

        //GET ALL Purchase Types
        public async Task<List<PurchaseTypeDTO>> GetAllAsync(string? search, int page, int limit)
        {
            var list = new List<PurchaseTypeDTO>();

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("[Purchase].[sp_PurchaseType_GetAll]", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Search", (object?)search ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Page", page);
            cmd.Parameters.AddWithValue("@Limit", limit);

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                list.Add(new PurchaseTypeDTO
                {
                    PurchaseTypeId = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    IsActive = reader.GetBoolean(2)
                });
            }

            return list;
        }


        //UPDATE Purchase Type
        public async Task<bool> UpdateAsync(int id, PurchaseTypeUpdate dto)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("[Purchase].[sp_PurchaseType_Update]", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@PurchaseTypeID", id);
            cmd.Parameters.AddWithValue("@Name", dto.Name);

            await conn.OpenAsync();

            // MUST use ExecuteScalarAsync because the SP returns a value
            var result = await cmd.ExecuteScalarAsync();

            int rowsAffected = result != null ? Convert.ToInt32(result) : 0;

            return rowsAffected > 0;
        }


        //CHANGE STATUS Purchase Type (IsActive)
        public async Task<bool> ChangeStatusAsync(int id, PurchaseTypeStatus dto)
        {
            if (id != dto.PurchaseTypeId)
                throw new ArgumentException("Purchase Type ID in URL does not match ID in body.");

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("[Purchase].[sp_PurchaseType_ChangeStatus]", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@PurchaseTypeID", id);
            cmd.Parameters.AddWithValue("@IsActive", dto.IsActive);

            await conn.OpenAsync();
            var rowsAffected = await cmd.ExecuteNonQueryAsync();

            return rowsAffected > 0;
        }
    }
}
