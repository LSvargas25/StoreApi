using Microsoft.Data.SqlClient;
using StoreApi.Interface.Tax;
using StoreApi.ModelsDTO.Tax;
using System.Data;

namespace StoreApi.Services.Tax
{
    public class TaxService : ITaxService
    {
        private readonly string _connectionString;

        public TaxService(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("StoreDb");
        }

        // CREATE NEW TAX
        public async Task<int> CreateAsync(TaxDTO dto)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("[Tax].[sp_Tax_Create]", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Name", dto.Name);
            cmd.Parameters.AddWithValue("@Percentage", dto.Percentage);

            var output = new SqlParameter("@NewTaxID", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };

            cmd.Parameters.Add(output);

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();

            return (int)output.Value;
        }

        // DELETE TAX BY ID
        public async Task<bool> DeleteAsync(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("[Tax].[sp_Tax_Delete]", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@TaxID", id);

            await conn.OpenAsync();
            var result = await cmd.ExecuteScalarAsync();

            return (int)(result ?? 0) > 0;
        }

        // GET ALL TAXES WITH PAGINATION
        public async Task<List<TaxDTO>> GetAllAsync(string? search, int page, int limit)
        {
            // Prevent SQL errors from FETCH 0
            if (page <= 0) page = 1;
            if (limit <= 0) limit = 10;

            var list = new List<TaxDTO>();

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("[Tax].[sp_Tax_GetAll]", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Search", (object?)search ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Page", page);
            cmd.Parameters.AddWithValue("@Limit", limit);

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                list.Add(new TaxDTO
                {
                    TaxId = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Percentage = reader.GetDecimal(2)
                });
            }

            return list;
        }

        // UPDATE TAX BY ID
        public async Task<bool> UpdateAsync(int id, TaxDTO dto)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("[Tax].[sp_Tax_Update]", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@TaxID", id);
            cmd.Parameters.AddWithValue("@Name", dto.Name);
            cmd.Parameters.AddWithValue("@Percentage", dto.Percentage);

            await conn.OpenAsync();

            var result = await cmd.ExecuteScalarAsync();

            return (int)(result ?? 0) > 0;
        }
    }
}
