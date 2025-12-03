namespace StoreApi.ModelsDTO.Inventory
{
    public class InventoryLedgerDTO
    {
        public int LedgerId { get; set; }

        public int InventoryTransactionId { get; set; }

        public int ItemVariantId { get; set; }

        public int WarehouseId { get; set; }

        public int OldQty { get; set; }

        public int NewQty { get; set; }

        public int QtyChange { get; set; }

        public decimal Cost { get; set; }

        public int RunningBalanceQty { get; set; }

        public decimal RunningBalanceCost { get; set; }

        public string? Method { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
