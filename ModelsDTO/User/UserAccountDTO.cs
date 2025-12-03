namespace StoreApi.ModelsDTO.User
{
    public class UserAccountDTO
    {
        public int UserId { get; set; }

        public string UserName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public byte[] PasswordHash { get; set; } = null!;

        public byte[] PasswordSalt { get; set; } = null!;

        public string? PhoneNumber { get; set; }

        public string? CardId { get; set; }

        public bool IsActive { get; set; }

        public int RoleId { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }


    }
}
