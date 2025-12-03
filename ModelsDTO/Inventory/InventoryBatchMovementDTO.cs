namespace StoreApi.ModelsDTO.Inventory
{
    public class InventoryBatchMovementDTO
    {
        public int InventoryBatchMovementId { get; set; }

        public int InventoryBatchId { get; set; }

        public int Quantity { get; set; }

        public string? MovementType { get; set; }

        public string? Reference { get; set; }

        public DateTime Date { get; set; }
    }
}
