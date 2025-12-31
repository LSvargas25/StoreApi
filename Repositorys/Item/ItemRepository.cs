using Microsoft.Data.SqlClient;
using StoreApi.ModelsDTO.Item;
using System.Data;

namespace StoreApi.Repositorys.Item
{
    public class ItemRepository : IItemRepository
    {
        private readonly string _connectionString;

        public ItemRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("StoreDb")!;
        }

        // ======================================================
        // CREATE
        // ======================================================
        public async Task<int> CreateAsync(ItemCreateDTO dto)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("[Item].[sp_Item_Create]", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Name", dto.Name);
            cmd.Parameters.AddWithValue("@Description", (object?)dto.Description ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Barcode", (object?)dto.Barcode ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Brand", (object?)dto.Brand ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Weight", (object?)dto.Weight ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Height", (object?)dto.Height ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Width", (object?)dto.Width ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Length", (object?)dto.Length ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@ItemCategoryId", (object?)dto.ItemCategoryId ?? DBNull.Value);

            var outputId = new SqlParameter("@NewItemId", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(outputId);

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();

            return (int)outputId.Value;
        }

        // ======================================================
        // UPDATE
        // ======================================================
        public async Task<bool> UpdateAsync(int itemId, ItemUpdateDTO dto)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("[Item].[sp_Item_Update]", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ItemId", itemId);
            cmd.Parameters.AddWithValue("@Name", dto.Name);
            cmd.Parameters.AddWithValue("@Description", (object?)dto.Description ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Barcode", (object?)dto.Barcode ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Brand", (object?)dto.Brand ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Weight", (object?)dto.Weight ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Height", (object?)dto.Height ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Width", (object?)dto.Width ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Length", (object?)dto.Length ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@ItemCategoryId", (object?)dto.ItemCategoryId ?? DBNull.Value);

            await conn.OpenAsync();
            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        // ======================================================
        // CHANGE STATUS
        // ======================================================
        public async Task<bool> ChangeStatusAsync(int itemId, bool isActive)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("[Item].[sp_Item_ChangeStatus]", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ItemId", itemId);
            cmd.Parameters.AddWithValue("@IsActive", isActive);

            await conn.OpenAsync();

            var result = await cmd.ExecuteScalarAsync();
            return result != null && Convert.ToBoolean(result);
        }


        // ======================================================
        // GET BY ID
        // ======================================================
        public async Task<ItemDTO?> GetByIdAsync(int itemId)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("[Item].[sp_Item_GetById]", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ItemId", itemId);

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            if (!reader.Read()) return null;

            return new ItemDTO
            {
                ItemId = reader.GetInt32("ItemID"),
                Name = reader.GetString("Name"),
                Description = reader["Description"] as string,
                Barcode = reader["Barcode"] as string,
                Brand = reader["Brand"] as string,
                Weight = reader["Weight"] as decimal?,
                Height = reader["Height"] as decimal?,
                Width = reader["Width"] as decimal?,
                Length = reader["Length"] as decimal?,
                IsActive = reader.GetBoolean("IsActive"),
                ItemCategoryId = reader["ItemCategoryID"] as int?,
                CreatedAt = reader.GetDateTime("CreatedAt"),
                UpdatedAt = reader.IsDBNull(reader.GetOrdinal("UpdatedAt"))
                    ? null
                    : reader.GetDateTime(reader.GetOrdinal("UpdatedAt"))
            };
        }





        // ======================================================
        // GET ALL
        // ======================================================
        public async Task<List<ListItem>> GetAllAsync(string? search)
        {
            var list = new List<ListItem>();

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("[Item].[sp_Item_GetAll]", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Search", (object?)search ?? DBNull.Value);

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                list.Add(new ListItem
                {
                    ItemId = reader.GetInt32("ItemID"),
                    Name = reader.GetString("Name"),
                    Url = reader["Url"] as string,
                    Barcode = reader["Barcode"] as string
                });
            }

            return list;
        }

        // ======================================================
        // GET ALL WITH ATTRIBUTES
        // ======================================================
        public async Task<List<ListItemWithAttribute>> GetAllWithAttributesAsync(string? search)
        {
            var list = new List<ListItemWithAttribute>();

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("[Item].[sp_Item_GetAll_WithAttributes]", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Search", (object?)search ?? DBNull.Value);

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                list.Add(new ListItemWithAttribute
                {
                    ItemId = reader.GetInt32("ItemID"),
                    Name = reader.GetString("Name"),
                    Url = reader["Url"] as string,
                    Barcode = reader["Barcode"] as string,
                    AttributeName = reader.GetString("AttributeName"),
                    Value = reader.GetString("Value"),
                    IsFavorite = reader.GetBoolean("IsFavorite")
                });
            }

            return list;
        }

        // ======================================================
        // DELETE
        // ======================================================
        public async Task<bool> DeleteAsync(int itemId)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("[Item].[sp_Item_Delete]", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ItemId", itemId);

            await conn.OpenAsync();
            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        public async Task<ItemFullResponseDTO?> GetFullByIdAsync(int itemId)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("[Item].[Item_List]", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ItemId", itemId);

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
                return null;

            var item = new ItemFullResponseDTO
            {
                ItemId = reader.GetInt32("ItemId"),
                Name = reader.GetString("Name"),
                Description = reader["Description"] as string,
                Barcode = reader["Barcode"] as string,
                Brand = reader["Brand"] as string,
                Weight = reader["Weight"] as decimal?,
                Height = reader["Height"] as decimal?,
                Width = reader["Width"] as decimal?,
                Length = reader["Length"] as decimal?,
                Category = new ItemCategoryDTO
                {
                    ItemCategoryId = reader.GetInt32("ItemCategoryId"),
                    Name = reader.GetString("CategoryName")
                }
            };

            // SEGUNDO RESULTSET → IMÁGENES
            await reader.NextResultAsync();
            while (await reader.ReadAsync())
            {
                item.Images.Add(new ItemImageDTO
                {
                    ItemImageId = reader.GetInt32("ItemImageId"),
                    Url = reader.GetString("Url"),
                    IsPrimary = reader.GetBoolean("IsPrimary")
                });
            }

            // TERCER RESULTSET → ATRIBUTOS
            await reader.NextResultAsync();
            while (await reader.ReadAsync())
            {
                item.Attributes.Add(new AttributeDetailDTO
                {
                    AttributeId = reader.GetInt32("AttributeId"),
                    Value = reader.GetString("Value")
                });
            }

            return item;
        }
    }
}
