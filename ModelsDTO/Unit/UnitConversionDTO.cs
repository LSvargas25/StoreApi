namespace StoreApi.ModelsDTO.Unit
{
    public class UnitConversionDTO
    {
        public int UnitConversionId { get; set; }
        public int FromUnitId { get; set; }
        public int ToUnitId { get; set; }
        public decimal Factor { get; set; }
    }

    public class UnitConversionCreateDTO
    {
        public int FromUnitId { get; set; }
        public int ToUnitId { get; set; }
        public decimal Factor { get; set; }
    }

    public class UnitConversionRequestDTO
    {
        public int FromUnitId { get; set; }
        public int ToUnitId { get; set; }
        public decimal Value { get; set; }
    }
    namespace StoreApi.ModelsDTO.Unit
    {
        public class UnitConversionResultDTO
        {
            public decimal OriginalValue { get; set; }
            public decimal ResultValue { get; set; }

            public string FromUnitName { get; set; } = null!;
            public string ToUnitName { get; set; } = null!;
        }
    }


}
