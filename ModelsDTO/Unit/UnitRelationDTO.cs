namespace StoreApi.ModelsDTO.Unit
{
    public class UnitRelationDTO
    {
        public int UnitRelationId { get; set; }
        public int UnitId { get; set; }
        public int ItemVariantId { get; set; }
        public decimal Value { get; set; }
        public bool IsFavorite { get; set; }
    }

    public class UnitRelationCreateDTO
    {
        public int UnitId { get; set; }
        public int ItemVariantId { get; set; }
        public decimal Value { get; set; }
        public bool IsFavorite { get; set; }
    }


    public class UnitRelationStatus
    {
        public bool IsActive { get; set; }
    }
}
