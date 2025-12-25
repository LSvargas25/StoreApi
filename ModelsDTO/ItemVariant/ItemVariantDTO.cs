using System;
using System.ComponentModel.DataAnnotations;

namespace StoreApi.ModelsDTO.ItemVariant
{
    // ======================================================
    // MAIN DTO (READ)
    // ======================================================
    public class ItemVariantDTO
    {
        public int ItemVariantId { get; set; }

        public string Name { get; set; } = null!;

        public string? Sku { get; set; }

        public decimal StandardPrice { get; set; }

        public decimal StandardCost { get; set; }

        public string? Barcode { get; set; }

        public bool IsActive { get; set; }

        public int ItemId { get; set; }

        public DateTime CreatedAt { get; set; }

        // Nullable por regla de negocio: solo se setea si hubo update
        public DateTime? UpdatedAt { get; set; }
    }

    // ======================================================
    // CREATE
    // ======================================================
    public class ItemVariantCreateDTO
    {
        [Required]
        [StringLength(150)]
        public string Name { get; set; } = null!;

        [StringLength(80)]
        public string? Sku { get; set; }

        [Range(0, 999999999)]
        public decimal StandardPrice { get; set; }

        [Range(0, 999999999)]
        public decimal StandardCost { get; set; }

        [StringLength(80)]
        public string? Barcode { get; set; }

        [Range(1, int.MaxValue)]
        public int ItemId { get; set; }
    }

    // ======================================================
    // UPDATE
    // ======================================================
    public class ItemVariantUpdateDTO
    {
        [Required]
        [StringLength(150)]
        public string Name { get; set; } = null!;

        [StringLength(80)]
        public string? Sku { get; set; }

        [Range(0, 999999999)]
        public decimal StandardPrice { get; set; }

        [Range(0, 999999999)]
        public decimal StandardCost { get; set; }

        [StringLength(80)]
        public string? Barcode { get; set; }
    }

    // ======================================================
    // SIMPLE LIST (EF)
    // ======================================================
    public class ListVariantDTO
    {
        public int ItemVariantId { get; set; }

        public string Name { get; set; } = null!;

        public string? Sku { get; set; }

        public decimal StandardPrice { get; set; }

        public decimal StandardCost { get; set; }

        public string? Barcode { get; set; }

        public bool IsActive { get; set; }
    }

    // ======================================================
    // COMPLETE LIST (SP)
    // ======================================================
    public class ListCompleteVariantDTO
    {
        public int ItemVariantId { get; set; }

        public string Name { get; set; } = null!;

        public string? Sku { get; set; }

        public decimal StandardPrice { get; set; }

        public decimal StandardCost { get; set; }

        public string? Barcode { get; set; }

        public bool IsActive { get; set; }

        // Viene de relación con unidades (nullable)
        public decimal? Value { get; set; }

        public string UnitName { get; set; } = null!;

        // Ej: favorito del usuario
        public bool? IsFavorite { get; set; }
    }

    // ======================================================
    // PRICE / COST LIST (SP)
    // ======================================================
    public class ListPriceCost
    {
        public int ItemVariantId { get; set; }

        public string Name { get; set; } = null!;

        public decimal StandardPrice { get; set; }

        public decimal StandardCost { get; set; }
    }

    // ======================================================
    // CHANGE STATUS
    // ======================================================
    public class ItemVariantStatus
    {
        public bool IsActive { get; set; }
    }

    // ======================================================
    // LIST BY ITEM ID (EF)
    // ======================================================
    public class ListItemVariantByItemIdDTO
    {
        public int ItemVariantId { get; set; }

        public string ItemVariantName { get; set; } = null!;

        public int ItemId { get; set; }

        public string ItemName { get; set; } = null!;

        public string? Url { get; set; }
    }

    // ======================================================
    // UPDATE PRICE (AUDIT)
    // ======================================================
    public class UpdateVariantPriceDTO
    {
        [Range(0, 999999999)]
        public decimal NewSalePrice { get; set; }

        // Opcional: solo si quieres guardar el costo en el historial de precio
        [Range(0, 999999999)]
        public decimal? NewCost { get; set; }

        [Required]
        [StringLength(250)]
        public string Description { get; set; } = null!;

        [Range(1, int.MaxValue)]
        public int CreatedByUserAccountId { get; set; }
    }

    // ======================================================
    // UPDATE COST (AUDIT)
    // ======================================================
    public class UpdateVariantCostDTO
    {
        [Range(0, 999999999)]
        public decimal NewCost { get; set; }

        [Required]
        [StringLength(250)]
        public string Reason { get; set; } = null!;

        [Range(1, int.MaxValue)]
        public int MethodId { get; set; }

        [Range(1, int.MaxValue)]
        public int ChangedByUserId { get; set; }
    }

    // ======================================================
    // PRICE HISTORY
    // ======================================================
    public class PriceHistoryDTO
    {
        public int PriceHistoryId { get; set; }

        public int ItemVariantId { get; set; }

        public DateTime CreatedAt { get; set; }

        public string? Description { get; set; }

        public decimal? Cost { get; set; }

        public decimal SalePrice { get; set; }

        public int CreatedByUserAccountId { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }

    // ======================================================
    // COST HISTORY
    // ======================================================
    public class ItemCostHistoryDTO
    {
        public int ItemCostHistoryId { get; set; }

        public int ItemVariantId { get; set; }

        public int MethodId { get; set; }

        public decimal OldCost { get; set; }

        public decimal NewCost { get; set; }

        public string? Reason { get; set; }

        public DateTime ChangedAt { get; set; }

        public int ChangedByUserId { get; set; }
    }

    //Item stats DTO
    public class ItemStatsDTO
    {
        public int TotalProducts { get; set; }
        public int ActiveProducts { get; set; }
        public int InactiveProducts { get; set; }
    }


}
