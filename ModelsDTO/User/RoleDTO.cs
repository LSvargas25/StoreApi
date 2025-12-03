namespace StoreApi.ModelsDTO.User
{
    public class RoleDTO
    {
        public int RoleId { get; set; }

        public string Name { get; set; } = null!;

        public bool IsActive { get; set; }
    }
}
