namespace StoreApi.ModelsDTO.Item
{
    public class ItemCategoryDTO
    {

        public int ItemCategoryId { get; set; }

        public string Name { get; set; } = null!;

        public bool IsActive { get; set; }
    }
    //create categroy 
    public class ItemCatCreate
    {
        public int ItemCategoryId { get; set; }
        public string Name { get; set; } = null!;
    }

    //update category 
    public class ItemCatUpdt
    {
        public int ItemCategoryId { get; set; }

        public string Name { get; set; } = null!;

    }
    //change status 
    public class ItemChangStatus{

        public int ItemCategoryId { get; set; }
        public bool IsActive { get; set; }

    }


    

}
