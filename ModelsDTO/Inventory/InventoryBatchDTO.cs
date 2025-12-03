namespace StoreApi.ModelsDTO.Inventory
{
    public class InventoryBatchDTO
    {
        public int InventoryBatchId { get; set; }

        public string BatchNumber { get; set; } = null!;

        public int ItemVariantId { get; set; }

        public int WarehouseId { get; set; }

        public int Quantity { get; set; }

        public DateOnly? ManufactureDate { get; set; }

    }
}
