namespace StoreApi.ModelsDTO.Invoice
{
    public class InvoiceDTO
    {
        public int InvoiceId { get; set; }

        public decimal Total { get; set; }

        public int CurrencyId { get; set; }

        public decimal ExchangeRate { get; set; }

        public bool IsActive { get; set; }

        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public int? InvoiceTypeId { get; set; }
    }
}
