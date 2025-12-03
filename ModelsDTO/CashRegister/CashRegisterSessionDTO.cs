namespace StoreApi.ModelsDTO.CashRegister
{
    public class CashRegisterSessionDTO
    {
        public int CashRegisterSessionId { get; set; }

        public int CashRegisterId { get; set; }

        public DateTime OpenDate { get; set; }

        public decimal OpenAmount { get; set; }

        public DateTime? CloseDate { get; set; }

        public decimal? CloseAmount { get; set; }

        public bool IsActive { get; set; }

        public int? CreatedByUserId { get; set; }
    }
}
