namespace StoreApi.ModelsDTO.User
{
    public class UserWareHouseDTO
    {
        public int UserWareHouseID { get; set; }

        public int UserID { get; set; }

        public int WarehouseID { get; set; }

        public bool IsActive { get; set; }
    }

    public class CreateUserWareHouse
    {
        public int UserID { get; set; }

        public int WarehouseID { get; set; }

        public bool IsActive { get; set; }
    }

    public class status
    {

                public bool IsActive { get; set; }
    }
    public class UpdateStatus
    {
        public int UserWareHouseID { get; set; }
        public bool IsActive { get; set; }
    }

}
