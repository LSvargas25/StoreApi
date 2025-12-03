namespace StoreApi.ModelsDTO.Discount
{
    public class DiscountDTO
    {
        public int DiscountId { get; set; }

        public string Name { get; set; } = null!;

        public decimal Percentage { get; set; }

    }
}
