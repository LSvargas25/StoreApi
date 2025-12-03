namespace StoreApi.ModelsDTO.hook
{
    public class WebhookEventDTO
    {
        public int WebhookEventId { get; set; }

        public int OutboxMessageId { get; set; }

        public int SubscriberId { get; set; }

        public string Status { get; set; } = null!;

        public DateTime? LastAttemptAt { get; set; }

        public int? InvoiceId { get; set; }

        public int? SaleId { get; set; }
    }
}
