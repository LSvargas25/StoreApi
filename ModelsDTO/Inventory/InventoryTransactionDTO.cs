namespace StoreApi.ModelsDTO.Inventory
{
    public class InventoryTransactionDTO
    {
        public int InventoryTransactionId { get; set; }

        public decimal Amount { get; set; }

        public int Quantity { get; set; }

        public DateTime Date { get; set; }

        public bool IsActive { get; set; }

        public int TransactionTypeId { get; set; }

        public int InventoryWarehouseId { get; set; }

        public int? CreatedByUserId { get; set; }

        public string? Notes { get; set; }
    }
}
