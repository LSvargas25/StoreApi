namespace StoreApi.ModelsDTO.Exchange
{
    public class ExchangeRateDTO
    {
        public int ExchangeRateId { get; set; }

        public int CurrencyId { get; set; }

        public decimal Rate { get; set; }

        public DateTime Date { get; set; }
    }
}
