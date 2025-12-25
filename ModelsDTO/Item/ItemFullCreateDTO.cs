using StoreApi.ModelsDTO.Item.StoreApi.ModelsDTO.Item;

namespace StoreApi.ModelsDTO.Item
{
    public class ItemFullCreateDTO
    {
        // Datos principales
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? Barcode { get; set; }
        public string? Brand { get; set; }

        // Dimensiones
        public decimal? Weight { get; set; }
        public decimal? Height { get; set; }
        public decimal? Width { get; set; }
        public decimal? Length { get; set; }

        public int ItemCategoryId { get; set; }

        // Relaciones
        public List<ItemImageCreateDTO> Images { get; set; } = [];
        public List<AttributeCreateDetailDTO> Attributes { get; set; } = [];
    }


}
