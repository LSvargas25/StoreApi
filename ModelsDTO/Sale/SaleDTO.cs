namespace StoreApi.ModelsDTO.Sale
{
    public class SaleDTO
    {
        public int SaleId { get; set; }

        public DateTime SaleDate { get; set; }

        public string? Notes { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public int? CreatedByUserId { get; set; }

        public int? CustomerId { get; set; }

        public int? InvoiceId { get; set; }

        public int CurrencyId { get; set; }
    }
}
