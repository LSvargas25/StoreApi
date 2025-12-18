namespace StoreApi.ModelsDTO.Item
{
    public class AttributeDTO
    {
        public int AttributeId { get; set; }

        public string Name { get; set; } = null!;

    }
    public class AttributeCreateDTO
    {
        public string Name { get; set; } = null!;

    }
}
