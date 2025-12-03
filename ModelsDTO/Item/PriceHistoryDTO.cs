namespace StoreApi.ModelsDTO.Item
{
    public class PriceHistoryDTO
    {
        public int PriceHistoryId { get; set; }

        public DateTime Date { get; set; }

        public string? Description { get; set; }

        public decimal Cost { get; set; }

        public decimal SalePrice { get; set; }

        public int ItemId { get; set; }

    }
}
