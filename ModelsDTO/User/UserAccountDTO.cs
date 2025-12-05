namespace StoreApi.ModelsDTO.User
{
    public class UserAccountCreateDTO
    {
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public string? CardId { get; set; }
        public int RoleId { get; set; }
    }

    public class UserAccountDTO
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public string? CardId { get; set; }
        public bool IsActive { get; set; }
        public int RoleId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
    public class UserUpdateDTO
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public string? Password { get; set; }
        public string? CardId { get; set; }  

    }
    public class UserActiveDTO
    {
        public int UserId { get; set; }
        public bool IsActive { get; set; }
    }
    public class UserRoleDTO
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
    }
}
