using Microsoft.Data.SqlClient;
using StoreApi.Interface.Item;
using StoreApi.ModelsDTO.Customer;
using StoreApi.ModelsDTO.Item;
using System.Data;

namespace StoreApi.Services.Item
{
    public class ItemCategoryService:IItemCategoryService
    {
        private readonly string _connectionString;
        public ItemCategoryService(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("StoreDb");
        }

        public async Task<bool> ChangeStatusAsync(int id, ItemChangStatus dto)
        {
            if (id != dto.ItemCategoryId)
                throw new ArgumentException("Item Category ID in URL does not match ID in body.");

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("[Item].[sp_ItemType_ChangeStatus]", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ItemTypeID", id);
            cmd.Parameters.AddWithValue("@IsActive", dto.IsActive);

            await conn.OpenAsync();

            // Must read SELECT @@ROWCOUNT
            var result = await cmd.ExecuteScalarAsync();
            int rowsAffected = result != null ? Convert.ToInt32(result) : 0;

            return rowsAffected > 0;
        }


        //Create a new Item category 
        public async Task<int> CreateAsync(ItemCatCreate dto)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("[Item].[sp_ItemType_Create]", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Name", dto.Name);

            var output = new SqlParameter("@NewItemTypeID", System.Data.SqlDbType.Int)
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
            using var cmd = new SqlCommand("[Item].[sp_ItemType_Delete]", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ItemTypeID", id);

            await conn.OpenAsync();
            var rows = await cmd.ExecuteScalarAsync();

            return (int)(rows ?? 0) > 0;
        }

        public async Task<List<ItemCategoryDTO>> GetAllAsync(string? search, int page, int limit)
        {
            var list = new List<ItemCategoryDTO>();

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("[Item].[sp_ItemType_GetAll]", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Search", (object?)search ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Page", page);
            cmd.Parameters.AddWithValue("@Limit", limit);

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                list.Add(new ItemCategoryDTO
                {
                    ItemCategoryId = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    IsActive = reader.GetBoolean(2)  
                });
            }

            return list;
        }


        public async Task<bool> UpdateAsync(int id, ItemCatUpdt dto)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("[Item].[sp_ItemType_Update]", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ItemCategoryID", id);
            cmd.Parameters.AddWithValue("@Name", dto.Name);

            await conn.OpenAsync();

            // Read SELECT @@ROWCOUNT;
            var result = await cmd.ExecuteScalarAsync();
            int rowsAffected = result != null ? Convert.ToInt32(result) : 0;

            return rowsAffected > 0;
        }




    }
}
