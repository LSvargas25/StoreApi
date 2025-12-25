using Microsoft.Data.SqlClient;
using StoreApi.Interface.Item;
using StoreApi.ModelsDTO.Item;
using System.Data;

namespace StoreApi.Services.Item
{
    public class ItemAttributeDetailService : IItemAttributeDetailService
    {
        private readonly string _connectionString;

        public ItemAttributeDetailService(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("StoreDb")!;
        }

        // ======================================================
        // GET ALL BY ITEM
        // ======================================================
        public async Task<List<AttributeDetailDTO>> GetAllAttributeDetailsByItemIdAsync(int itemId)
        {
            var list = new List<AttributeDetailDTO>();

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("[Item].[sp_AttributeDetail_GetAll]", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ItemId", itemId);

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                list.Add(new AttributeDetailDTO
                {
                    ItemId = reader.GetInt32(reader.GetOrdinal("ItemId")),
                    AttributeId = reader.GetInt32(reader.GetOrdinal("AttributeId")),
                    Value = reader.GetString(reader.GetOrdinal("Value")),
                    IsFavorite = reader.GetBoolean(reader.GetOrdinal("IsFavorite"))
                });
            }

            return list;
        }

        // ======================================================
        // CREATE
        // ======================================================
        public async Task<bool> CreateAttributeDetailAsync(
            int itemId,
            AttributeCreateDetailDTO attributeDetail)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("[Item].[sp_AttributeDetail_Create]", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ItemId", itemId);
            cmd.Parameters.AddWithValue("@AttributeId", attributeDetail.AttributeId);
            cmd.Parameters.AddWithValue("@Value", attributeDetail.Value);
            cmd.Parameters.AddWithValue("@IsFavorite", attributeDetail.IsFavorite);

            var output = new SqlParameter("@NewAttributeDetailID", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };

            cmd.Parameters.Add(output);

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();

            return Convert.ToInt32(output.Value) > 0;
        }

        // ======================================================
        // UPDATE
        // ======================================================
        public async Task<bool> UpdateAttributeDetailAsync(
            int itemId,
            int attributeId,
            AttributeCreateDetailDTO attributeDetail)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("[Item].[sp_AttributeDetail_Update]", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ItemId", itemId);
            cmd.Parameters.AddWithValue("@AttributeId", attributeId);
            cmd.Parameters.AddWithValue("@Value", attributeDetail.Value);

            await conn.OpenAsync();
            var rows = await cmd.ExecuteScalarAsync();

            return Convert.ToInt32(rows) > 0;
        }

        // ======================================================
        // DELETE
        // ======================================================
        public async Task<bool> DeleteAttributeDetailAsync(int itemId, int attributeId)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("[Item].[sp_AttributeDetail_Delete]", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ItemId", itemId);
            cmd.Parameters.AddWithValue("@AttributeId", attributeId);

            await conn.OpenAsync();
            var rows = await cmd.ExecuteScalarAsync();

            return Convert.ToInt32(rows) > 0;
        }

        // ======================================================
        // CHANGE FAVORITE STATUS
        // ======================================================
        public async Task<bool> ChangeFavoriteStatusAsync(
            int itemId,
            ChangeFavoriteStatusDTO statusDto)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("[Item].[sp_AttributeDetail_ChangeFavorite]", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ItemId", itemId);
            cmd.Parameters.AddWithValue("@AttributeId", statusDto.AttributeId);
            cmd.Parameters.AddWithValue("@IsFavorite", statusDto.IsFavorite);

            await conn.OpenAsync();
            var rows = await cmd.ExecuteScalarAsync();

            return Convert.ToInt32(rows) > 0;
        }

        // ======================================================
        // RESULT (LOOKUP)
        // ======================================================
        public async Task<List<ResultDTO>> GetResultAsync(string? search)
        {
            var list = new List<ResultDTO>();

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(
                "[Item].[sp_AttributeDetail_GetAll_ForResult]",
                conn);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Search", (object?)search ?? DBNull.Value);

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                list.Add(new ResultDTO
                {
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    Value = reader.GetString(reader.GetOrdinal("Value")),
                    IsFavorite = reader.GetBoolean(reader.GetOrdinal("IsFavorite"))
                });
            }

            return list;
        }
    }
}
