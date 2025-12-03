namespace StoreApi.ModelsDTO.Payment
{
    public class PaymentDetailDTO
    {
        public int PaymentDetailId { get; set; }

        public string? Reference { get; set; }

        public decimal Amount { get; set; }

        public int PaymentId { get; set; }
    }
}
