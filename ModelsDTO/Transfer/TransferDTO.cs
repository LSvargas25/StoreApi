namespace StoreApi.ModelsDTO.Transfer
{
    public class TransferDTO
    {
        public int TransferId { get; set; }

        public int FromWarehouseId { get; set; }

        public int ToWarehouseId { get; set; }

        public DateTime Date { get; set; }

        public bool IsActive { get; set; }

        public int? CreatedByUserId { get; set; }

        public string? Description { get; set; }
    }
}
