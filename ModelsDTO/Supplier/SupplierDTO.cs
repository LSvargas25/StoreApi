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

  public class SupplierStatus
    {
        public int SupplierId { get; set; }
        public bool IsActive { get; set; }
    }
 public class SupplierUpdate
    {
        public int SupplierId { get; set; }
        public string Name { get; set; } = null!;

        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }
    }

    public class SupplierRole
    {
        public int SupplierId { get; set; }

        public int? SupplierTypeId { get; set; }
    }
    public class SupplierTypeDTO
    {
        public int SupplierTypeId { get; set; }

        public string Name { get; set; } = null!;
    }

}
