using Microsoft.Data.SqlClient;
using StoreApi.ModelsDTO.Unit;
using System.Data;

namespace StoreApi.Repositorys.Unit
{
    public class UnitRelationRepository : IUnitRelationRepository
    {
        private readonly string _connectionString;

        public UnitRelationRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        // ======================================================
        // CREATE
        // ======================================================
        public async Task<int> CreateAsync(UnitRelationCreateDTO dto)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("[Unit].[sp_UnitRelation_Create]", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ItemVariantId", dto.ItemVariantId);
            cmd.Parameters.AddWithValue("@UnitId", dto.UnitId);
            cmd.Parameters.AddWithValue("@Value", dto.Value);
            cmd.Parameters.AddWithValue("@IsFavorite", dto.IsFavorite);

            var output = new SqlParameter("@NewUnitRelationId", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(output);

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();

            var newId = (int)output.Value;

            if (newId == -1)
                throw new Exception("This unit is already assigned to the item variant.");

            return newId;

        }

        // ======================================================
        // GET ALL
        // ======================================================
        public async Task<List<UnitRelationDTO>> GetAllForViewAsync(string? search)
        {
            var list = new List<UnitRelationDTO>();

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("[Unit].[sp_UnitRelation_GetAll]", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Search", (object?)search ?? DBNull.Value);

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                list.Add(new UnitRelationDTO
                {
                    UnitRelationId = reader.GetInt32("UnitRelationId"),
                    ItemVariantId = reader.GetInt32("ItemVariantId"),
                    UnitId = reader.GetInt32("UnitId"),
                    Value = reader.GetDecimal("Value"),
                    IsFavorite = reader.GetBoolean("IsFavorite")
                });
            }

            return list;
        }

        // ======================================================
        // UPDATE
        // ======================================================
        public async Task<bool> UpdateAsync(int id, UnitRelationCreateDTO dto)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("[Unit].[sp_UnitRelation_Update]", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@UnitRelationId", id);
            cmd.Parameters.AddWithValue("@UnitId", dto.UnitId);
            cmd.Parameters.AddWithValue("@Value", dto.Value);
            cmd.Parameters.AddWithValue("@IsFavorite", dto.IsFavorite);

            await conn.OpenAsync();
            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        // ======================================================
        // DELETE
        // ======================================================
        public async Task<bool> DeleteAsync(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("[Unit].[sp_UnitRelation_Delete]", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@UnitRelationId", id);

            await conn.OpenAsync();
            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        // ======================================================
        // CHANGE STATUS
        // ======================================================
        public async Task<bool> ChangeStatusAsync(int id, UnitRelationStatus dto)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("[Unit].[sp_UnitRelation_ChangeStatus]", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@UnitRelationId", id);
            cmd.Parameters.AddWithValue("@IsActive", dto.IsActive);

            await conn.OpenAsync();
            return await cmd.ExecuteNonQueryAsync() > 0;
        }
    }
}
