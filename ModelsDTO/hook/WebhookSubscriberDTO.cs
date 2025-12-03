namespace StoreApi.ModelsDTO.hook
{
    public class WebhookSubscriberDTO
    {
        public int WebhookSubscriberId { get; set; }

        public string Url { get; set; } = null!;

        public string? Events { get; set; }

        public bool IsActive { get; set; }

    }
}
