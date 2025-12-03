namespace StoreApi.ModelsDTO.ItemVariant
{
    public class UnitRelationDTO
    {
        public int UnitRelationId { get; set; }

        public int UnitId { get; set; }

        public int ItemVariantId { get; set; }

        public string Value { get; set; } = null!;

        public bool IsFavorite { get; set; }

    }
}
