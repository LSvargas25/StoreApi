using Microsoft.Data.SqlClient;
using StoreApi.Interface.Supplier;
using StoreApi.ModelsDTO.Supplier;
using StoreApi.ModelsDTO.User;
using System.Data;


namespace StoreApi.Services.Supplier
{
    public class SupplierTypeService:ISupplierTypeService
    {
        private readonly string _connectionString;
       
        public SupplierTypeService(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("StoreDb");
        }

        //GET ALL THE SUPPLIER TYPE 
        public async Task<List<SupplierTypeDTO>> GetAllAsync(string? search, int page, int limit)
        {
            var list = new List<SupplierTypeDTO>();

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("[Supplier].[sp_SupplierType_GetAll]", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Search", (object?)search ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Page", page);
            cmd.Parameters.AddWithValue("@Limit", limit);

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                list.Add(new SupplierTypeDTO
                {
                    SupplierTypeId = reader.GetInt32(0),
                    Name = reader.GetString(1),
                });
            }

            return list;
        }

        //CREATE A NEW SUPPLIER TYPE 
        public async Task<int> CreateAsync(SupplierTypeDTO dto)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("[Supplier].[sp_SupplierType_Create]", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Name", dto.Name);

            var output = new SqlParameter("@NewSupplierTypeID", System.Data.SqlDbType.Int)
            {
                Direction = System.Data.ParameterDirection.Output
            };

            cmd.Parameters.Add(output);

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();

            return (int)output.Value;
        }

        //DELETE SUPPLIER TYPE
        public async Task<bool> DeleteAsync(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("[Supplier].[sp_SupplierType_Delete]", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@SupplierTypeID", id);

            await conn.OpenAsync();
            var rows = await cmd.ExecuteScalarAsync();

            return (int)(rows ?? 0) > 0;
        }

        public async Task<bool> UpdateAsync(int id, SupplierTypeDTO dto)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("[Supplier].[sp_SupplierType_Update]", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@SupplierTypeID", id);
            cmd.Parameters.AddWithValue("@Name", dto.Name);

            await conn.OpenAsync();

             
            var result = await cmd.ExecuteScalarAsync();

            return (int)(result ?? 0) > 0;
        }


    }
}
