namespace StoreApi.ModelsDTO.Invoice
{
    public class InvoiceDetailDTO
    {
        public int InvoiceDetailId { get; set; }

        public int InvoiceId { get; set; }

        public DateTime Date { get; set; }

        public decimal TaxAmount { get; set; }

        public decimal DiscountAmount { get; set; }

        public decimal TotalLine { get; set; }

        public decimal Total { get; set; }

        public int? DiscountId { get; set; }

        public int? TaxId { get; set; }

        public decimal CostAtMovement { get; set; }

        public int? InventoryTransactionId { get; set; }

        public int? InventoryBatchId { get; set; }

        public int ItemVariantId { get; set; }
    }
}
