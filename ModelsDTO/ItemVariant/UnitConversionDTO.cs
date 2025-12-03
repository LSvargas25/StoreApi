namespace StoreApi.ModelsDTO.ItemVariant
{
    public class UnitConversionDTO
    {
        public int UnitConversionId { get; set; }

        public int FromUnitId { get; set; }

        public int ToUnitId { get; set; }

        public decimal Factor { get; set; }
    }
}
