namespace StoreApi.ModelsDTO.Purshase
{
    public class PurchaseTypeDTO
    {
        public int PurchaseTypeId { get; set; }

        public string Name { get; set; } = null!;

        public bool IsActive { get; set; }
    }
}
