using Microsoft.Data.SqlClient;
using StoreApi.Interface.Item;
using StoreApi.ModelsDTO.Item;
using System.Data;

namespace StoreApi.Services.Item
{
    public class ItemAttributeService:IItemAttributeService
    {
        private readonly string _connectionString;

        public ItemAttributeService(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("StoreDb");
        }

        public async Task<int> CreateAttributeAsync(AttributeCreateDTO attribute)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("[Item].[sp_Attribute_Create]", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Name", attribute.Name);

            var output = new SqlParameter("@NewAttributeID", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };

            cmd.Parameters.Add(output);

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();

            return (int)output.Value;
        }


        public async Task<bool> DeleteAttributeAsync(int id)
        {

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("[Item].[sp_Attribute_Delete]", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@AttributeID", id);

            await conn.OpenAsync();
            var rows = await cmd.ExecuteScalarAsync();

            return (int)(rows ?? 0) > 0;
        }

        public async Task<List<AttributeDTO>> GetAllAttributesAsync(string? search)
        {
            var list = new List<AttributeDTO>();

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("[Item].[sp_Attribute_GetAll]", conn);
            cmd.CommandType = CommandType.StoredProcedure;

             
            cmd.Parameters.AddWithValue("@Search", (object?)search ?? DBNull.Value);

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                list.Add(new AttributeDTO
                {
                    AttributeId = reader.GetInt32(reader.GetOrdinal("AttributeID")),
                    Name = reader.GetString(reader.GetOrdinal("Name"))
                });
            }

            return list;
        }




        public async Task<bool> UpdateAttributeAsync(int id, AttributeCreateDTO attribute)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("[Item].[sp_Attribute_Update]", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@AttributeID", id);
            cmd.Parameters.AddWithValue("@Name", attribute.Name);

            await conn.OpenAsync();

            // Read SELECT @@ROWCOUNT;
            var result = await cmd.ExecuteScalarAsync();
            int rowsAffected = result != null ? Convert.ToInt32(result) : 0;

            return rowsAffected > 0;
        }
    }
}
