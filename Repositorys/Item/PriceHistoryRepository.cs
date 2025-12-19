using Microsoft.Data.SqlClient;
using StoreApi.ModelsDTO.Item;
using System.Data;

namespace StoreApi.Repositorys.Item
{
    public class PriceHistoryRepository : IPriceHistoryRepository
    {
        private readonly string _connectionString;

        public PriceHistoryRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("StoreDb")!;
        }

        // ======================================================
        // CREATE
        // ======================================================
        public async Task<int> CreateAsync(PriceHistoryCreateDTO dto)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("[Item].[sp_PriceHistory_Create]", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@ItemVariantId", SqlDbType.Int).Value = dto.ItemVariantId;
            cmd.Parameters.Add("@Description", SqlDbType.NVarChar).Value =
                (object?)dto.Description ?? DBNull.Value;
            cmd.Parameters.Add("@Cost", SqlDbType.Decimal).Value = dto.Cost;
            cmd.Parameters.Add("@SalePrice", SqlDbType.Decimal).Value = dto.SalePrice;
            cmd.Parameters.Add("@CreatedByUserAccountId", SqlDbType.Int).Value =
                dto.CreatedByUserAccountId;

            var output = new SqlParameter("@NewPriceHistoryId", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(output);

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();

            return (int)output.Value;
        }
 
        // ======================================================
        // HISTORY
        // ======================================================
        public async Task<List<PriceHistoryDTO>> GetByVariantAsync(
            int itemVariantId,
            DateTime? from,
            DateTime? to)
        {
            var list = new List<PriceHistoryDTO>();

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("[Item].[sp_PriceHistory_GetByVariant]", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@ItemVariantId", SqlDbType.Int).Value = itemVariantId;
            cmd.Parameters.Add("@From", SqlDbType.DateTime).Value =
                (object?)from ?? DBNull.Value;
            cmd.Parameters.Add("@To", SqlDbType.DateTime).Value =
                (object?)to ?? DBNull.Value;

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                list.Add(new PriceHistoryDTO
                {
                    PriceHistoryId = reader.GetInt32("PriceHistoryId"),
                    Date = DateOnly.FromDateTime(reader.GetDateTime("CreatedAt")),
                    Description = reader.IsDBNull("Description")
                        ? null
                        : reader.GetString("Description"),
                    Cost = reader.GetDecimal("Cost"),
                    SalePrice = reader.GetDecimal("SalePrice"),
                    ItemVariantId = reader.GetInt32("ItemVariantId"),
                    ItemName = reader.GetString("ItemName"),
                    VariantName = reader.GetString("VariantName")
                });
            }

            return list;
        }

        // ======================================================
        // DASHBOARD / AUDIT / ADMIN
        // ======================================================
  


        public Task<List<ListPriceByUserDTO>> ListPriceByUserAsync(int userAccountId)
            => ExecuteListAsync<ListPriceByUserDTO>(
                "[Item].[sp_PriceHistory_PriceByUser]", userAccountId);

        public async Task<List<ListAllPriceHistoryDTO>> ListAllAsync(
            string? search,
            int page,
            int limit)
        {
            var list = new List<ListAllPriceHistoryDTO>();

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("[Item].[sp_PriceHistory_GetAll]", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@Search", SqlDbType.NVarChar).Value =
                (object?)search ?? DBNull.Value;
            cmd.Parameters.Add("@Page", SqlDbType.Int).Value = page;
            cmd.Parameters.Add("@Limit", SqlDbType.Int).Value = limit;

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                list.Add(new ListAllPriceHistoryDTO
                {
                    Date = DateOnly.FromDateTime(reader.GetDateTime("CreatedAt")),
                    Description = reader.IsDBNull("Description")
                        ? null
                        : reader.GetString("Description"),
                    Cost = reader.GetDecimal("Cost"),
                    SalePrice = reader.GetDecimal("SalePrice"),
                    ItemName = reader.GetString("ItemName"),
                    VariantName = reader.GetString("VariantName"),
                    CreatedBy = reader.GetString("CreatedBy")
                });
            }

            return list;
        }

        // ======================================================
        // Helper genérico
        // ======================================================
        private async Task<List<T>> ExecuteListAsync<T>(string sp, object param)
            where T : new()
        {
            var list = new List<T>();

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sp, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Search", param);

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                list.Add((T)Activator.CreateInstance(typeof(T), reader)!);
            }

            return list;
        }
    }
}
