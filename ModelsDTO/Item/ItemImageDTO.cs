namespace StoreApi.ModelsDTO.Item
{
    public class ItemImageDTO
    {
        public int ItemImageId { get; set; }

        public string Url { get; set; } = null!;

        public bool IsPrimary { get; set; }

        public int ItemId { get; set; }
    }
    public class ItemImageUploadDTO
    {
       
        public IFormFile File { get; set; }
         
        public int ItemId { get; set; }

        public bool IsMain { get; set; }
    }
    namespace StoreApi.ModelsDTO.Item
    {
        public class ItemImageCreateDTO
        {
            public string Url { get; set; }
            public bool IsPrimary { get; set; }
        }
    }


}
