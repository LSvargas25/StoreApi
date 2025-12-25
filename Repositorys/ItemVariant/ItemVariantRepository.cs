using System.Data;
using Microsoft.Data.SqlClient;
using StoreApi.ModelsDTO.ItemVariant;

namespace StoreApi.Repositorys.ItemVariant
{
    public class ItemVariantRepository : IItemVariantRepository
    {
        private readonly string _connectionString;

        public ItemVariantRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("StoreDb");
        }

        public async Task<int> CreateAsync(ItemVariantCreateDTO dto)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("[ItemVariant].[sp_ItemVariant_Create]", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ItemId", dto.ItemId);
            cmd.Parameters.AddWithValue("@Name", dto.Name.Trim());
            cmd.Parameters.AddWithValue("@Sku", (object?)dto.Sku?.Trim() ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Barcode", (object?)dto.Barcode?.Trim() ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@StandardPrice", dto.StandardPrice);
            cmd.Parameters.AddWithValue("@StandardCost", dto.StandardCost);

            var output = new SqlParameter("@NewItemVariantId", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(output);

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();

            return (int)output.Value;
        }

        public async Task<ItemVariantDTO?> GetByIdAsync(int itemVariantId)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("[ItemVariant].[sp_ItemVariant_GetById]", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ItemVariantId", itemVariantId);

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
                return null;

            return new ItemVariantDTO
            {
                ItemVariantId = reader.GetInt32(reader.GetOrdinal("ItemVariantId")),
                ItemId = reader.GetInt32(reader.GetOrdinal("ItemId")),
                Name = reader.GetString(reader.GetOrdinal("Name")),
                Sku = reader["Sku"] as string,
                Barcode = reader["Barcode"] as string,
                StandardPrice = reader.GetDecimal(reader.GetOrdinal("StandardPrice")),
                StandardCost = reader.GetDecimal(reader.GetOrdinal("StandardCost")),
                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                UpdatedAt = reader["UpdatedAt"] == DBNull.Value ? null : reader.GetDateTime(reader.GetOrdinal("UpdatedAt"))
            };
        }

        public async Task<bool> UpdateAsync(int itemVariantId, ItemVariantUpdateDTO dto)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("[ItemVariant].[sp_ItemVariant_Update]", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ItemVariantId", itemVariantId);
            cmd.Parameters.AddWithValue("@Name", dto.Name.Trim());
            cmd.Parameters.AddWithValue("@Sku", (object?)dto.Sku?.Trim() ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Barcode", (object?)dto.Barcode?.Trim() ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@StandardPrice", dto.StandardPrice);
            cmd.Parameters.AddWithValue("@StandardCost", dto.StandardCost);

            await conn.OpenAsync();
            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int itemVariantId)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("[ItemVariant].[sp_ItemVariant_Delete]", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ItemVariantId", itemVariantId);

            await conn.OpenAsync();
            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        public async Task<bool> ChangeStatusAsync(int itemVariantId, bool isActive)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("[ItemVariant].[sp_ItemVariant_ChangeStatus]", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ItemVariantId", itemVariantId);
            cmd.Parameters.AddWithValue("@IsActive", isActive);

            await conn.OpenAsync();
            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        public async Task<List<ListCompleteVariantDTO>> GetCompleteListAsync(int itemId)
        {
            var list = new List<ListCompleteVariantDTO>();

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("[ItemVariant].[sp_ItemVariant_ListCompleteByItemId]", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ItemId", itemId);

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                list.Add(new ListCompleteVariantDTO
                {
                    ItemVariantId = reader.GetInt32(reader.GetOrdinal("ItemVariantId")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    Sku = reader["Sku"] as string,
                    Barcode = reader["Barcode"] as string,
                    StandardPrice = reader.GetDecimal(reader.GetOrdinal("StandardPrice")),
                    StandardCost = reader.GetDecimal(reader.GetOrdinal("StandardCost")),
                    IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                    Value = reader["Value"] == DBNull.Value ? null : reader.GetDecimal(reader.GetOrdinal("Value")),
                    UnitName = reader.GetString(reader.GetOrdinal("UnitName")),
                    IsFavorite = reader["IsFavorite"] == DBNull.Value ? null : reader.GetBoolean(reader.GetOrdinal("IsFavorite"))
                });
            }

            return list;
        }

        public async Task<List<ListPriceCost>> GetPriceCostListAsync(int itemId)
        {
            var list = new List<ListPriceCost>();

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("[ItemVariant].[sp_ItemVariant_ListPriceCostByItemId]", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ItemId", itemId);

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                list.Add(new ListPriceCost
                {
                    ItemVariantId = reader.GetInt32(reader.GetOrdinal("ItemVariantId")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    StandardPrice = reader.GetDecimal(reader.GetOrdinal("StandardPrice")),
                    StandardCost = reader.GetDecimal(reader.GetOrdinal("StandardCost"))
                });
            }

            return list;
        }
    



 public async Task<bool> UpdatePriceAsync(int itemVariantId, UpdateVariantPriceDTO dto)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("[ItemVariant].[sp_ItemVariant_UpdatePrice]", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ItemVariantId", itemVariantId);
            cmd.Parameters.AddWithValue("@NewSalePrice", dto.NewSalePrice);
            cmd.Parameters.AddWithValue("@NewCost", (object?)dto.NewCost ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Description", dto.Description.Trim());
            cmd.Parameters.AddWithValue("@CreatedByUserAccountId", dto.CreatedByUserAccountId);

            await conn.OpenAsync();
            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        public async Task<bool> UpdateCostAsync(int itemVariantId, UpdateVariantCostDTO dto)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("[ItemVariant].[sp_ItemVariant_UpdateCost]", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ItemVariantId", itemVariantId);
            cmd.Parameters.AddWithValue("@MethodId", dto.MethodId);
            cmd.Parameters.AddWithValue("@NewCost", dto.NewCost);
            cmd.Parameters.AddWithValue("@Reason", dto.Reason.Trim());
            cmd.Parameters.AddWithValue("@ChangedByUserId", dto.ChangedByUserId);

            await conn.OpenAsync();
            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        public async Task<List<PriceHistoryDTO>> GetPriceHistoryAsync(int itemVariantId)
        {
            var list = new List<PriceHistoryDTO>();

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("[ItemVariant].[sp_PriceHistory_ListByItemVariantId]", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ItemVariantId", itemVariantId);

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                list.Add(new PriceHistoryDTO
                {
                    PriceHistoryId = reader.GetInt32(reader.GetOrdinal("PriceHistoryID")),
                    ItemVariantId = reader.GetInt32(reader.GetOrdinal("ItemVariantID")),
                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                    Description = reader["Description"] as string,
                    Cost = reader["Cost"] == DBNull.Value ? null : reader.GetDecimal(reader.GetOrdinal("Cost")),
                    SalePrice = reader.GetDecimal(reader.GetOrdinal("SalePrice")),
                    CreatedByUserAccountId = reader.GetInt32(reader.GetOrdinal("CreatedByUserAccountID")),
                    UpdatedAt = reader["UpdatedAt"] == DBNull.Value ? null : reader.GetDateTime(reader.GetOrdinal("UpdatedAt"))
                });
            }

            return list;
        }

        public async Task<List<ItemCostHistoryDTO>> GetCostHistoryAsync(int itemVariantId)
        {
            var list = new List<ItemCostHistoryDTO>();

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("[ItemVariant].[sp_ItemCostHistory_ListByItemVariantId]", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ItemVariantId", itemVariantId);

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                list.Add(new ItemCostHistoryDTO
                {
                    ItemCostHistoryId = reader.GetInt32(reader.GetOrdinal("ItemCostHistoryID")),
                    ItemVariantId = reader.GetInt32(reader.GetOrdinal("ItemVariantID")),
                    MethodId = reader.GetInt32(reader.GetOrdinal("MethodID")),
                    OldCost = reader.GetDecimal(reader.GetOrdinal("OldCost")),
                    NewCost = reader.GetDecimal(reader.GetOrdinal("NewCost")),
                    Reason = reader["Reason"] as string,
                    ChangedAt = reader.GetDateTime(reader.GetOrdinal("ChangedAt")),
                    ChangedByUserId = reader.GetInt32(reader.GetOrdinal("ChangedByUserID"))
                });
            }

            return list;
        }

        public async Task<ItemStatsDTO> GetItemStatsAsync()
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("[ItemVariant].[sp_ItemVariant_GetStats]", conn);

            cmd.CommandType = CommandType.StoredProcedure;

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
                return new ItemStatsDTO();

            return new ItemStatsDTO
            {
                TotalProducts = reader.GetInt32(reader.GetOrdinal("TotalProducts")),
                ActiveProducts = reader.GetInt32(reader.GetOrdinal("ActiveProducts")),
                InactiveProducts = reader.GetInt32(reader.GetOrdinal("InactiveProducts"))
            };
        }



    }
}
