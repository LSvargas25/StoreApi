namespace StoreApi.ModelsDTO.Invoice
{
    public class InvoiceVersionDTO
    {
        public int InvoiceVersionId { get; set; }

        public int InvoiceId { get; set; }

        public int VersionNumber { get; set; }

        public string DataSnapshot { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public int? CreatedByUserId { get; set; }
    }
}
