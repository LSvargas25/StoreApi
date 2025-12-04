namespace StoreApi.ModelsDTO.User
{
    public class UserLoginResponseDTO
    {
        public string Token { get; set; }
        public UserAccountDTO User { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
