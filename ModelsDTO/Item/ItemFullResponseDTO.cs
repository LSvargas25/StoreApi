namespace StoreApi.ModelsDTO.Item
{
    public class ItemFullResponseDTO
    {
        public int ItemId { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? Barcode { get; set; }
        public string? Brand { get; set; }

        public decimal? Weight { get; set; }
        public decimal? Height { get; set; }
        public decimal? Width { get; set; }
        public decimal? Length { get; set; }

        public ItemCategoryDTO Category { get; set; } = null!;
        public List<ItemImageDTO> Images { get; set; } = [];
        public List<AttributeDetailDTO> Attributes { get; set; } = [];
    }
}
