namespace StoreApi.ModelsDTO.Currency
{
    public class CurrencyDTO
    {
        public int CurrencyId { get; set; }

        public string Code { get; set; } = null!;

        public string Name { get; set; } = null!;

        public bool IsDefault { get; set; }
    }
}
