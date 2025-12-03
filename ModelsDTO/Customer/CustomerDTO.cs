namespace StoreApi.ModelsDTO.Customer
{
    public class CustomerDTO
    {
        public int CustomerId { get; set; }

        public string Name { get; set; } = null!;

        public string? CardId { get; set; }

        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Address { get; set; }

        public bool IsActive { get; set; }

        public int? CustomerRoleId { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
