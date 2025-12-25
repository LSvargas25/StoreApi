namespace StoreApi.ModelsDTO.Item
{
    public class ItemVersionDTO
    {
        public int ItemVersionId { get; set; }

        public int ItemId { get; set; }

        public int VersionNumber { get; set; }

        public string DataSnapshot { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public int? CreatedByUserId { get; set; }

    }
    public class ItemVersionCreateDTO
    {
        public int ItemId { get; set; }

        public int VersionNumber { get; set; }

        public string DataSnapshot { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public int? CreatedByUserId { get; set; }
    }
}
