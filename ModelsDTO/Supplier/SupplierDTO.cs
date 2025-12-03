namespace StoreApi.ModelsDTO.Supplier
{
    public class SupplierDTO
    {
        public int SupplierId { get; set; }

        public string Name { get; set; } = null!;

        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }

        public bool IsActive { get; set; }

        public int? SupplierTypeId { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
