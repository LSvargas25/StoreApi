namespace StoreApi.ModelsDTO.Customer
{
    public class CustomerRoleDTO
    {
        public int CustomerRoleId { get; set; }

        public string Name { get; set; } = null!;

        public bool IsActive { get; set; }
    }
    public class CustomerRoleCreat
    {
        public int CustomerRoleId { get; set; }

        public string Name { get; set; } = null!;
    }
    public class CustomerRoleUpdt
    {
        public int CustomerRoleId { get; set; }
        public string Name { get; set; } = null!;
    }
    public class CustomerRoleChangs
    {
        public int CustomerRoleId { get; set; }
        public bool IsActive { get; set; }

    }


}
