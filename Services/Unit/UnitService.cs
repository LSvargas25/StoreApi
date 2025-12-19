using Microsoft.Data.SqlClient;
using StoreApi.Interface.Unit;
using StoreApi.ModelsDTO.Item;
using StoreApi.ModelsDTO.Unit;
using System.Data;

namespace StoreApi.Services.Unit
{
    public class UnitService: IUnitService
    {
        private readonly  string _connectionString;

        public UnitService(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("StoreDb");
        }


        //delete a unit
        public async Task<bool> DeleteUnitAsync(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("[Unit].[sp_Unit_Delete]", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@UnitID", id);

            await conn.OpenAsync();
            var rows = await cmd.ExecuteScalarAsync();

            return (int)(rows ?? 0) > 0;
        }
        //get all units

        public async Task<List<UnitDTO>> GetAllUnitsAsync(string? search)
        {

            var list = new List<UnitDTO>();

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("[Unit].[sp_Unit_GetAll]", conn);
            cmd.CommandType = CommandType.StoredProcedure;


            cmd.Parameters.AddWithValue("@Search", (object?)search ?? DBNull.Value);

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                list.Add(new UnitDTO
                {
                    UnitId = reader.GetInt32(reader.GetOrdinal("UnitID")),
                    Name = reader.GetString(reader.GetOrdinal("Name"))
                });
            }

            return list;
        }


        //update a unit
        public async Task<bool> UpdateUnitAsync(int id, UnitCreateDTO dto)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("[Unit].[sp_Unit_Update]", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@UnitID", id);
            cmd.Parameters.AddWithValue("@Name", dto.Name);

            await conn.OpenAsync();


            var result = await cmd.ExecuteScalarAsync();

            return (int)(result ?? 0) > 0;
        }

        //create a new unit
        async Task<int> IUnitService.CreateUnitAsync(UnitCreateDTO dto)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("[Unit].[sp_Unit_Create]", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Name", dto.Name);
            var output = new SqlParameter("@NewUnitID", System.Data.SqlDbType.Int)
            {
                Direction = System.Data.ParameterDirection.Output
            };
            cmd.Parameters.Add(output);

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();

            return (int)output.Value;
        }
    }
}
