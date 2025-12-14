namespace StoreApi.ModelsDTO.Item
{
    public class ItemDTO
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

        public bool IsActive { get; set; }

        public int? ItemCategoryId { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
    public class ItemCreateDTO
    {
    

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public string? Barcode { get; set; }

        public string? Brand { get; set; }

        public decimal? Weight { get; set; }

        public decimal? Height { get; set; }

        public decimal? Width { get; set; }

        public decimal? Length { get; set; }

        public int? ItemCategoryId { get; set; }

    }

    public class ItemUpdateDTO
    {

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public string? Barcode { get; set; }

        public string? Brand { get; set; }

        public decimal? Weight { get; set; }

        public decimal? Height { get; set; }

        public decimal? Width { get; set; }

        public decimal? Length { get; set; }

    }

    public class ItemChangeStatus
    {

        public bool IsActive { get; set; }
    }




}
