namespace StoreApi.ModelsDTO.Credit
{
    public class CreditPaymentDTO
    {
        public int CreditPaymentId { get; set; }

        public decimal Amount { get; set; }

        public DateTime Date { get; set; }

        public int? CreatedByUserId { get; set; }

        public int CreditId { get; set; }
    }
}
