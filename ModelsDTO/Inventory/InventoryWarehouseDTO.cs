namespace StoreApi.ModelsDTO.Inventory
{
    public class InventoryWarehouseDTO
    {
        public int InventoryWarehouseId { get; set; }

        public bool IsActive { get; set; }

        public int ActualStock { get; set; }

        public int MinStock { get; set; }

        public int MaxStock { get; set; }

        public int WarehouseId { get; set; }

        public int ItemVariantId { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
