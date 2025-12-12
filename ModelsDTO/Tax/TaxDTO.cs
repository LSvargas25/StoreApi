namespace StoreApi.ModelsDTO.Tax
{
    public class TaxDTO
    {
        public int TaxId { get; set; }

        public string Name { get; set; } = null!;

        public decimal Percentage { get; set; }
    }
      
}
