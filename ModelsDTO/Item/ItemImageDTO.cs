namespace StoreApi.ModelsDTO.Item
{
    public class ItemImageDTO
    {
        public int ItemImageId { get; set; }

        public string Url { get; set; } = null!;

        public bool IsPrimary { get; set; }

        public int ItemId { get; set; }
    }
}
