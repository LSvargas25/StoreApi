using Microsoft.Data.SqlClient;
using StoreApi.Interface.Unit;
using StoreApi.ModelsDTO.Unit;
using System.Data;

namespace StoreApi.Repositorys.Unit
{
    public class UnitConversionRepository : IUnitConversionRepository
    {
        private readonly string _connectionString;

        public UnitConversionRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("StoreDb");
        }

        public async Task<int> CreateAsync(UnitConversionCreateDTO dto)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("[Unit].[sp_UnitConversion_Create]", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@FromUnitID", dto.FromUnitId);
            cmd.Parameters.AddWithValue("@ToUnitID", dto.ToUnitId);
            cmd.Parameters.AddWithValue("@Factor", dto.Factor);

            var output = new SqlParameter("@NewUnitConversionID", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(output);

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
            return (int)output.Value;
        }

        public async Task<List<UnitConversionDTO>> GetAllAsync(string? search)
        {
            var list = new List<UnitConversionDTO>();

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("[Unit].[sp_UnitConversion_GetAll]", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Search", (object?)search ?? DBNull.Value);

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                list.Add(new UnitConversionDTO
                {
                    UnitConversionId = reader.GetInt32("UnitConversionID"),
                    FromUnitId = reader.GetInt32("FromUnitID"),
                    ToUnitId = reader.GetInt32("ToUnitID"),
                    Factor = reader.GetDecimal("Factor")
                });
            }

            return list;
        }

        public async Task<bool> UpdateAsync(int id, UnitConversionCreateDTO dto)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("[Unit].[sp_UnitConversion_Update]", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@UnitConversionID", id);
            cmd.Parameters.AddWithValue("@FromUnitID", dto.FromUnitId);
            cmd.Parameters.AddWithValue("@ToUnitID", dto.ToUnitId);
            cmd.Parameters.AddWithValue("@Factor", dto.Factor);

            await conn.OpenAsync();
            return (int)(await cmd.ExecuteScalarAsync() ?? 0) > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("[Unit].[sp_UnitConversion_Delete]", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@UnitConversionID", id);

            await conn.OpenAsync();
            return (int)(await cmd.ExecuteScalarAsync() ?? 0) > 0;
        }

        public async Task<decimal?> GetFactorAsync(int fromUnitId, int toUnitId)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("[Unit].[sp_UnitConversion_GetFactor]", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@FromUnitID", fromUnitId);
            cmd.Parameters.AddWithValue("@ToUnitID", toUnitId);

            await conn.OpenAsync();
            return cmd.ExecuteScalar() as decimal?;
        }

        public Task<List<UnitConversionDTO>> GetAllConversionsAsync()
            => GetAllAsync(null);

        public async Task<string?> GetUnitNameAsync(int unitId)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("[Unit].[sp_Unit_GetNameById]", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@UnitID", unitId);

            await conn.OpenAsync();
            return cmd.ExecuteScalar() as string;
        }
    }
}
