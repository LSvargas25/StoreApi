namespace StoreApi.ModelsDTO.Item
{
    public class AttributeDetailDTO
    {
        public int ItemId { get; set; }

        public int AttributeId { get; set; }

        public string Value { get; set; } = null!;

        public bool IsFavorite { get; set; }

    }
}
