namespace StoreApi.ModelsDTO.Item
{
    public class ItemCategoryDTO
    {

        public int ItemCategoryId { get; set; }

        public string Name { get; set; } = null!;

        public bool IsActive { get; set; }
    }
}
