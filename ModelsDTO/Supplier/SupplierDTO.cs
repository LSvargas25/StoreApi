using System.ComponentModel.DataAnnotations;

namespace StoreApi.ModelsDTO.Supplier
{
    public class SupplierDTO
    {
        public int SupplierId { get; set; }

        [Required]
        [MaxLength(255)]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$", ErrorMessage = "Name only can contain letters and spaces.")]
        public string Name { get; set; } = null!;

        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [MaxLength(255)]
        public string? Email { get; set; }

        [MaxLength(20)]
        [RegularExpression(@"^\d*$", ErrorMessage = "PhoneNumber must contain only digits.")]
        public string? PhoneNumber { get; set; }

        public bool IsActive { get; set; }

        public int? SupplierTypeId { get; set; }

        public DateOnly? CreatedAt { get; set; }
        public DateOnly? UpdatedAt { get; set; }

    }
    public class CreateSupplier
    {

        [Required]
        [MaxLength(255)]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$", ErrorMessage = "Name only can contain letters and spaces.")]
        public string Name { get; set; } = null!;

        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [MaxLength(255)]
        public string? Email { get; set; }

        [MaxLength(20)]
        [RegularExpression(@"^\d*$", ErrorMessage = "PhoneNumber must contain only digits.")]
        public string? PhoneNumber { get; set; }
        public int? SupplierTypeId { get; set; }
        public int SupplierId { get; internal set; }
    }

    public class SupplierStatus
    {
        public int SupplierId { get; set; }
        public bool IsActive { get; set; }
    }

    public class SupplierUpdate
    {
        public int SupplierId { get; set; }

        [Required]
        [MaxLength(255)]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$", ErrorMessage = "Name only can contain letters and spaces.")]
        public string Name { get; set; } = null!;

        [EmailAddress]
        [MaxLength(255)]
        public string? Email { get; set; }

        [MaxLength(20)]
        [RegularExpression(@"^\d*$", ErrorMessage = "PhoneNumber must contain only digits.")]
        public string? PhoneNumber { get; set; }
    }

    public class SupplierRole
    {
        public int SupplierId { get; set; }
        public int? SupplierTypeId { get; set; }
    }


//this is only for the type so we dont use in the supplierservice or controller
public class SupplierTypeDTO
    {
        public int SupplierTypeId { get; set; }

        public string Name { get; set; } = null!;
    }

}
