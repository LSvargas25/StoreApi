namespace StoreApi.Models
{
    public class UserWarehouse
    {
        public int UserWareHouseID { get; set; }

        public int UserID { get; set; }

        public int WarehouseID { get; set; }

        public bool IsActive { get; set; }
    }
}
