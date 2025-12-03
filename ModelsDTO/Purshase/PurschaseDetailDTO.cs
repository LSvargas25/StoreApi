namespace StoreApi.ModelsDTO.Purshase
{
    public class PurschaseDetailDTO
    {
        public int PurchaseDetailId { get; set; }

        public int PurchaseId { get; set; }

        public int ItemVariantId { get; set; }

        public decimal Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public int? DiscountId { get; set; }

        public int? TaxId { get; set; }

        public decimal TotalLine { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
