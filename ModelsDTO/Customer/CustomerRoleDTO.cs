namespace StoreApi.ModelsDTO.Customer
{
    public class CustomerRoleDTO
    {
        public int CustomerRoleId { get; set; }

        public string Name { get; set; } = null!;

        public bool IsActive { get; set; }
    }
}
