using Microsoft.Data.SqlClient;
using StoreApi.Interface.Item;
using StoreApi.ModelsDTO.Item;
using System.Data;
namespace StoreApi.Services.Item
{
    public class ItemImageService
    {
        private readonly string _connectionString;
        public ItemImageService(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("StoreDb");
        }

       

        //CREATE A NEW IMAGE
        public async Task<int> CreateAsync(ItemImageDTO dto)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("[Item].[sp_ItemImage_Create]", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@URL", dto.Url);

            var output = new SqlParameter("@NewItemImage", System.Data.SqlDbType.Int)
            {
                Direction = System.Data.ParameterDirection.Output
            };

            cmd.Parameters.Add(output);

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();

            return (int)output.Value;
        }

        //DELETE IMAGE
        public async Task<bool> DeleteAsync(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("[Item].[sp_ItemImage_Delete]", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ItemImageID", id);

            await conn.OpenAsync();
            var rows = await cmd.ExecuteScalarAsync();

            return (int)(rows ?? 0) > 0;
        }
        //SET PRIMARY A IMAGE
        public async Task<bool> SetAsPrimaryAsync(int imageId)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("[Item].[sp_ItemImage_SetPrimary]", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ImageID", imageId);

            await conn.OpenAsync();
            var result = await cmd.ExecuteScalarAsync();

            return (int)(result ?? 0) > 0;
        }


    }
}
