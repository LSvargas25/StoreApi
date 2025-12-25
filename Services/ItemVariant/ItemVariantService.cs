using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using StoreApi.Interface.ItemVariant;
using StoreApi.Models;
using StoreApi.ModelsDTO.ItemVariant;
using StoreApi.Repositorys.ItemVariant;

namespace StoreApi.Services.ItemVariant
{
    public class ItemVariantService : IItemVariantService
    {
        private readonly IItemVariantRepository _repo;
        private readonly StoreContext _context;  

        public ItemVariantService(IItemVariantRepository repo, StoreContext context)
        {
            _repo = repo;
            _context = context;
        }

        
        private static (bool Success, string Message, int HttpHint) MapSqlError(SqlException ex, string fallback)
        {
            // HttpHint: 400/404/409/500 (lo usaremos en controller)
            return ex.Number switch
            {
                50005 => (false, "Item not found.", 404),
                50006 => (false, "Item variant not created successfully: duplicate name for this item.", 409),
                50007 => (false, "Item variant not created successfully: duplicate SKU for this item.", 409),
                50008 => (false, "Item variant not created successfully: duplicate barcode.", 409),

                50011 => (false, "Item variant not updated successfully: name is required.", 400),
                50012 => (false, "Item variant not updated successfully: standard cost cannot be greater than standard price.", 400),
                50013 => (false, "Item variant not updated successfully: duplicate name for this item.", 409),

                50021 => (false, "Item variant not found.", 404),
                50022 => (false, "Price not updated successfully: new price must be greater than or equal to zero.", 400),
                50023 => (false, "Price not updated successfully: no changes detected.", 400),

                50031 => (false, "Cost not updated successfully: costing method not found.", 404),
                50032 => (false, "Cost not updated successfully: new cost must be greater than or equal to zero.", 400),
                50033 => (false, "Cost not updated successfully: no changes detected.", 400),

                _ => (false, fallback, 500)
            };
        }

        public async Task<(bool Success, string Message, int? NewId)> CreateAsync(ItemVariantCreateDTO dto)
        {
            if (dto.StandardCost > dto.StandardPrice)
                return (false, "Item variant not created successfully: standard cost cannot be greater than standard price.", null);

            try
            {
                var newId = await _repo.CreateAsync(dto);
                return newId > 0
                    ? (true, "Item variant created successfully.", newId)
                    : (false, "Item variant not created successfully.", null);
            }
            catch (SqlException ex)
            {
                var mapped = MapSqlError(ex, "Item variant not created successfully.");
                return (false, mapped.Message, null);
            }
        }

        public async Task<(bool Success, string Message, ItemVariantDTO? Data)> GetByIdAsync(int itemVariantId)
        {
            var data = await _repo.GetByIdAsync(itemVariantId);
            return data == null
                ? (false, "Item variant not found.", null)
                : (true, "Item variant retrieved successfully.", data);
        }

        public async Task<(bool Success, string Message)> UpdateAsync(int itemVariantId, ItemVariantUpdateDTO dto)
        {
            if (dto.StandardCost > dto.StandardPrice)
                return (false, "Item variant not updated successfully: standard cost cannot be greater than standard price.");

            try
            {
                var ok = await _repo.UpdateAsync(itemVariantId, dto);
                return ok
                    ? (true, "Item variant updated successfully.")
                    : (false, "Item variant not found.");
            }
            catch (SqlException ex)
            {
                var mapped = MapSqlError(ex, "Item variant not updated successfully.");
                return (false, mapped.Message);
            }
        }

        public async Task<(bool Success, string Message)> DeleteAsync(int itemVariantId)
        {
            var ok = await _repo.DeleteAsync(itemVariantId);
            return ok
                ? (true, "Item variant deleted successfully.")
                : (false, "Item variant not found.");
        }

        public async Task<(bool Success, string Message)> ChangeStatusAsync(int itemVariantId, bool isActive)
        {
            var ok = await _repo.ChangeStatusAsync(itemVariantId, isActive);
            return ok
                ? (true, "Item variant status updated successfully.")
                : (false, "Item variant not found.");
        }

        // EF listados simples
        public async Task<List<ListVariantDTO>> GetAllActiveAsync(string? search = null)
        {
            var q = _context.ItemVariants.AsNoTracking().Where(v => v.IsActive);

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();
                q = q.Where(v =>
                    v.Name.Contains(search) ||
                    (v.Sku != null && v.Sku.Contains(search)) ||
                    (v.Barcode != null && v.Barcode.Contains(search)));
            }

            return await q.OrderBy(v => v.Name)
                .Select(v => new ListVariantDTO
                {
                    ItemVariantId = v.ItemVariantId,
                    Name = v.Name,
                    Sku = v.Sku,
                    Barcode = v.Barcode,
                    StandardPrice = v.StandardPrice,
                    StandardCost = v.StandardCost,
                    IsActive = v.IsActive
                }).ToListAsync();
        }

        public async Task<List<ListVariantDTO>> GetAllInactiveAsync(string? search = null)
        {
            var q = _context.ItemVariants.AsNoTracking().Where(v => !v.IsActive);

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();
                q = q.Where(v =>
                    v.Name.Contains(search) ||
                    (v.Sku != null && v.Sku.Contains(search)) ||
                    (v.Barcode != null && v.Barcode.Contains(search)));
            }

            return await q.OrderBy(v => v.Name)
                .Select(v => new ListVariantDTO
                {
                    ItemVariantId = v.ItemVariantId,
                    Name = v.Name,
                    Sku = v.Sku,
                    Barcode = v.Barcode,
                    StandardPrice = v.StandardPrice,
                    StandardCost = v.StandardCost,
                    IsActive = v.IsActive
                }).ToListAsync();
        }

        public Task<List<ListCompleteVariantDTO>> GetCompleteListAsync(int itemId)
            => _repo.GetCompleteListAsync(itemId);

        public Task<List<ListPriceCost>> GetPriceCostListAsync(int itemId)
            => _repo.GetPriceCostListAsync(itemId);

        public async Task<List<ListItemVariantByItemIdDTO>> GetByItemIdAsync(int itemId)
        {
            return await _context.ItemVariants.AsNoTracking()
                .Where(v => v.ItemId == itemId)
                .OrderBy(v => v.Name)
                .Select(v => new ListItemVariantByItemIdDTO
                {
                    ItemVariantId = v.ItemVariantId,
                    ItemVariantName = v.Name,
                    ItemId = v.ItemId,
                    ItemName = v.Item.Name,
                    Url = v.Item.ItemImages.Where(img => img.IsPrimary).Select(img => img.Url).FirstOrDefault()
                })
                .ToListAsync();
        }

        // ===== NUEVO: precio/costo con historial =====

        public async Task<(bool Success, string Message)> UpdatePriceAsync(int itemVariantId, UpdateVariantPriceDTO dto)
        {
            try
            {
                var ok = await _repo.UpdatePriceAsync(itemVariantId, dto);
                return ok ? (true, "Price updated successfully.") : (false, "Item variant not found.");
            }
            catch (SqlException ex)
            {
                var mapped = MapSqlError(ex, "Price not updated successfully.");
                return (false, mapped.Message);
            }
        }

        public async Task<(bool Success, string Message)> UpdateCostAsync(int itemVariantId, UpdateVariantCostDTO dto)
        {
            try
            {
                var ok = await _repo.UpdateCostAsync(itemVariantId, dto);
                return ok ? (true, "Cost updated successfully.") : (false, "Item variant not found.");
            }
            catch (SqlException ex)
            {
                var mapped = MapSqlError(ex, "Cost not updated successfully.");
                return (false, mapped.Message);
            }
        }

        public async Task<(bool Success, string Message, List<PriceHistoryDTO>? Data)> GetPriceHistoryAsync(int itemVariantId)
        {
            var variant = await _repo.GetByIdAsync(itemVariantId);
            if (variant == null) return (false, "Item variant not found.", null);

            var data = await _repo.GetPriceHistoryAsync(itemVariantId);
            return (true, "Price history retrieved successfully.", data);
        }

        public async Task<(bool Success, string Message, List<ItemCostHistoryDTO>? Data)> GetCostHistoryAsync(int itemVariantId)
        {
            var variant = await _repo.GetByIdAsync(itemVariantId);
            if (variant == null) return (false, "Item variant not found.", null);

            var data = await _repo.GetCostHistoryAsync(itemVariantId);
            return (true, "Cost history retrieved successfully.", data);
        }

        public async Task<(bool Success, string Message, ItemStatsDTO? Data)> GetItemStatsAsync()
        {
            try
            {
                var stats = await _repo.GetItemStatsAsync();

                // Validación defensiva (por si el SP no devuelve filas)
                if (stats == null)
                {
                    return (
                        false,
                        "Product statistics could not be retrieved.",
                        null
                    );
                }

                return (
                    true,
                    "Product statistics retrieved successfully.",
                    stats
                );
            }
            catch (SqlException)
            {
                return (
                    false,
                    "An error occurred while retrieving product statistics.",
                    null
                );
            }
            catch (Exception)
            {
                return (
                    false,
                    "Unexpected error while retrieving product statistics.",
                    null
                );
            }
        }

    }
}
