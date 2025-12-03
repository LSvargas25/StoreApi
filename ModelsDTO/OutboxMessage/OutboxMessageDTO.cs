namespace StoreApi.ModelsDTO.OutboxMessage
{
    public class OutboxMessageDTO
    {
        public int OutboxMessageId { get; set; }

        public string EventType { get; set; } = null!;

        public string? Payload { get; set; }

        public string Status { get; set; } = null!;

        public int Retries { get; set; }

        public DateTime CreatedAt { get; set; }

        public int? InvoiceId { get; set; }

        public int? SaleId { get; set; }
    }
}
