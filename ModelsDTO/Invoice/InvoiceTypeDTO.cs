namespace StoreApi.ModelsDTO.Invoice
{
    public class InvoiceTypeDTO
    {
        public int InvoiceTypeId { get; set; }

        public string Name { get; set; } = null!;

        public bool IsActive { get; set; }
    }
}
