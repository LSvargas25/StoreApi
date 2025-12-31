namespace StoreApi.ModelsDTO.WareHouse
{
    public class WareHouseDTO
    {
        public int WarehouseId { get; set; }

        public string Name { get; set; } = null!;

        public string? Address { get; set; }

        public string? PhoneNumber { get; set; }

        public bool IsActive { get; set; }

        public DateOnly CreatedAt { get; set; }
    }

         public class WareHouseCreateDTO
         { 
             public string Name { get; set; } = null!;

             public string? Address { get; set; }

             public string? PhoneNumber { get; set; }

              public DateOnly CreatedAt { get; set; }


         }

     public class WareHouseUpdateDTO
     {
         public string Name { get; set; } = null!;
         public string? Address { get; set; }
         public string? PhoneNumber { get; set; }

       
    }

    public class WareHouseStatusDTO
    {
        public int WarehouseId { get; set; }
        public bool IsActive { get; set; }
    }

    public class HardDeleteResultDTO
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }


}
