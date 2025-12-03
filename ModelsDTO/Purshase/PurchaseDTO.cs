namespace StoreApi.ModelsDTO.Purshase
{
    public class PurchaseDTO
    {
        public int PurchaseId { get; set; }

        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public bool IsActive { get; set; }

        public decimal Total { get; set; }

        public int CurrencyId { get; set; }

        public decimal ExchangeRate { get; set; }

        public int? PurchaseTypeId { get; set; }

        public int? CreatedByUserId { get; set; }

        public int? SupplierId { get; set; }
    }
}
