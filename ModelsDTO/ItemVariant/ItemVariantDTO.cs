namespace StoreApi.ModelsDTO.ItemVariant
{
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

        public DateTime UpdatedAt { get; set; }
    }
}
