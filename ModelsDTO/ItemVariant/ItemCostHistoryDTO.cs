namespace StoreApi.ModelsDTO.ItemVariant
{
    public class ItemCostHistoryDTO
    {
        public int ItemCostHistoryId { get; set; }

        public int ItemVariantId { get; set; }

        public int MethodId { get; set; }

        public decimal? OldCost { get; set; }

        public decimal? NewCost { get; set; }

        public string? Reason { get; set; }

        public DateTime ChangedAt { get; set; }

        public int? ChangedByUserId { get; set; }

    }
}
