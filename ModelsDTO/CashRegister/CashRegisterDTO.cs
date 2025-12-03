namespace StoreApi.ModelsDTO.CashRegister
{
    public class CashRegisterDTO
    {
        public int CashRegisterId { get; set; }

        public string Name { get; set; } = null!;

        public bool IsActive { get; set; }

        public int? WarehouseId { get; set; }
    }
}
