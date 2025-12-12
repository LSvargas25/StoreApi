namespace StoreApi.ModelsDTO.Purshase
{
    public class PurchaseTypeDTO
    {
        public int PurchaseTypeId { get; set; }

        public string Name { get; set; } = null!;

        public bool IsActive { get; set; }
    }
    public class PurchaseTypeCreate
    {
        public string Name { get; set; } = null!;
    }
    public class PurchaseTypeUpdate
    {
        public int PurchaseTypeId { get; set; }

        public string Name { get; set; } = null!;
    }
    public class  PurchaseTypeStatus
    {
        public int PurchaseTypeId { get; set; }
        public bool IsActive { get; set; }
    }


}

 


   
