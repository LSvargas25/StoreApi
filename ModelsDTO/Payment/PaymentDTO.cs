namespace StoreApi.ModelsDTO.Payment
{
    public class PaymentDTO
    {
        public int PaymentId { get; set; }

        public DateTime Date { get; set; }

        public decimal TotalAmount { get; set; }

        public bool IsCreditPayment { get; set; }

        public int? CreatedByUserId { get; set; }

        public int? SupplierId { get; set; }

        public int? CustomerId { get; set; }

        public int? PurchaseId { get; set; }

        public int? InvoiceId { get; set; }


    }
}
