using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using StoreApi.Models;
using StoreApi.ModelsDTO.WareHouse;

namespace StoreApi.Repositorys.WareHouse
{
    public class WareHouseRepository : IWareHouseRepository
    {
        private readonly StoreContext _context;

        public WareHouseRepository(StoreContext context)
        {
            _context = context;
        }

        public async Task<int> CreateAsync(WareHouseCreateDTO dto)
        {
            await using var conn = (SqlConnection)_context.Database.GetDbConnection();
            if (conn.State != ConnectionState.Open) await conn.OpenAsync();

            await using var cmd = new SqlCommand("WareHouse.sp_WareHouse_Create", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@Name", dto.Name);
            cmd.Parameters.AddWithValue("@Address", (object?)dto.Address ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@PhoneNumber", (object?)dto.PhoneNumber ?? DBNull.Value); 

            var outId = new SqlParameter("@WarehouseId", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(outId);

            await cmd.ExecuteNonQueryAsync();
            return (int)outId.Value;
        }

        public async Task<bool> UpdateAsync(int id, WareHouseUpdateDTO dto)
        {
            await using var conn = (SqlConnection)_context.Database.GetDbConnection();
            if (conn.State != ConnectionState.Open) await conn.OpenAsync();

            await using var cmd = new SqlCommand("WareHouse.sp_WareHouse_Update", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@WarehouseId", id);
            cmd.Parameters.AddWithValue("@Name", dto.Name);
            cmd.Parameters.AddWithValue("@Address", (object?)dto.Address ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@PhoneNumber", (object?)dto.PhoneNumber ?? DBNull.Value); 

            var rows = await cmd.ExecuteNonQueryAsync();
            return rows > 0;
        }

        public async Task<bool> ChangeStatusAsync(int id, bool isActive)
        {
            await using var conn = (SqlConnection)_context.Database.GetDbConnection();
            if (conn.State != ConnectionState.Open) await conn.OpenAsync();

            await using var cmd = new SqlCommand("WareHouse.sp_WareHouse_ChangeStatus", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@WarehouseId", id);
            cmd.Parameters.AddWithValue("@IsActive", isActive);

            var rows = await cmd.ExecuteNonQueryAsync();
            return rows > 0;
        }

        public async Task<List<WareHouseDTO>> GetAllAsync(string? search)
        {
            var result = new List<WareHouseDTO>();

            await using var conn = (SqlConnection)_context.Database.GetDbConnection();
            if (conn.State != ConnectionState.Open) await conn.OpenAsync();

            await using var cmd = new SqlCommand("WareHouse.sp_WareHouse_GetAll", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@Search", (object?)search ?? DBNull.Value);

            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                result.Add(MapWarehouse(reader));
            }

            return result;
        }

        public async Task<WareHouseDTO?> GetByIdAsync(int id)
        {
            await using var conn = (SqlConnection)_context.Database.GetDbConnection();
            if (conn.State != ConnectionState.Open) await conn.OpenAsync();

            await using var cmd = new SqlCommand("WareHouse.sp_WareHouse_GetById", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@WarehouseId", id);

            await using var reader = await cmd.ExecuteReaderAsync();
            if (!await reader.ReadAsync())
                return null;

            return MapWarehouse(reader);
        }

      

        private static WareHouseDTO MapWarehouse(SqlDataReader reader)
        {
            // Asumiendo que tu SPs seleccionan estas columnas (WarehouseID, Name, Address, PhoneNumber, IsActive, Location, CreatedAt)
            return new WareHouseDTO
            {
                WarehouseId = reader.GetInt32(reader.GetOrdinal("WarehouseID")),
                Name = reader.GetString(reader.GetOrdinal("Name")),
                Address = reader.IsDBNull(reader.GetOrdinal("Address")) ? null : reader.GetString(reader.GetOrdinal("Address")),
                PhoneNumber = reader.IsDBNull(reader.GetOrdinal("PhoneNumber")) ? null : reader.GetString(reader.GetOrdinal("PhoneNumber")),
                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                CreatedAt = DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("CreatedAt")))
            };
        }

        public async Task<HardDeleteResultDTO> HardDeleteAsync(int warehouseId, int userId)
        {
            await using var conn = (SqlConnection)_context.Database.GetDbConnection();
            if (conn.State != ConnectionState.Open) await conn.OpenAsync();

            await using var cmd = new SqlCommand("WareHouse.sp_WareHouse_HardDelete", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@WarehouseId", warehouseId);
            cmd.Parameters.AddWithValue("@UserId", userId);

            try
            {
                await cmd.ExecuteNonQueryAsync();

                return new HardDeleteResultDTO
                {
                    Success = true,
                    Message = "La sucursal fue eliminada correctamente."
                };
            }
            catch (SqlException ex) when (ex.Number == 50001)
            {
                return new HardDeleteResultDTO
                {
                    Success = false,
                    Message = "La sucursal no existe."
                };
            }
            catch (SqlException ex) when (ex.Number == 50002)
            {
                return new HardDeleteResultDTO
                {
                    Success = false,
                    Message = "No se puede eliminar la sucursal porque tiene dependencias activas."
                };
            }
        }
    }
    }

