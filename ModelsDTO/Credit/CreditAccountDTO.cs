namespace StoreApi.ModelsDTO.Credit
{
    public class CreditAccountDTO
    {
        public int CreditId { get; set; }

        public decimal TotalCredit { get; set; }

        public decimal CurrentBalance { get; set; }

        public bool IsActive { get; set; }

        public int CustomerId { get; set; }
    }
}
