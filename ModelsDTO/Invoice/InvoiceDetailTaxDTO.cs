namespace StoreApi.ModelsDTO.Invoice
{
    public class InvoiceDetailTaxDTO
    {
        public int InvoiceDetailTaxId { get; set; }

        public int InvoiceDetailId { get; set; }

        public int TaxId { get; set; }

        public decimal Amount { get; set; }
    }
}
