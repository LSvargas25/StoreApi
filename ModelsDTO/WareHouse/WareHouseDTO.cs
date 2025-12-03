namespace StoreApi.ModelsDTO.WareHouse
{
    public class WareHouseDTO
    {
        public int WarehouseId { get; set; }

        public string Name { get; set; } = null!;

        public string? Address { get; set; }

        public string? PhoneNumber { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
