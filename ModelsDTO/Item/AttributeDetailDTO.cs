namespace StoreApi.ModelsDTO.Item
{
    public class AttributeDetailDTO
    {
        public int ItemId { get; set; }
        public int AttributeId { get; set; }
        public string Value { get; set; } = null!;
        public bool IsFavorite { get; set; }
    }

    public class AttributeCreateDetailDTO
    {
        public int AttributeId { get; set; }
        public string Value { get; set; } = null!;

        public bool IsFavorite { get; set; }
    }

    public class ChangeFavoriteStatusDTO
    {
        public int AttributeId { get; set; }
        public bool IsFavorite { get; set; }
    }

   
    public class ResultDTO
    {
        public string Name { get; set; } = null!;
        public string Value { get; set; } = null!;
        public bool IsFavorite { get; set; }
    }

    
}
