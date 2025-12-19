namespace StoreApi.ModelsDTO.Unit
{
    public class UnitDTO
    {
        public int UnitId { get; set; }

        public string Name { get; set; } = null!;
    }

    public class UnitCreateDTO
    {
        public string Name { get; set; } = null!;
    }   
}
