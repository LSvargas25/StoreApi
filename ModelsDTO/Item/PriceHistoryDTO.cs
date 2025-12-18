namespace StoreApi.ModelsDTO.Item
{
    public class PriceHistoryDTO
    {
        public int PriceHistoryId { get; set; }
        public DateOnly Date { get; set; }
        public string? Description { get; set; }
        public decimal Cost { get; set; }
        public decimal SalePrice { get; set; }

        public int ItemVariantId { get; set; }
        public string ItemName { get; set; } = null!;
        public string VariantName { get; set; } = null!;
    }

    public class PriceHistoryCreateDTO
    {
        public string? Description { get; set; }
        public decimal Cost { get; set; }
        public decimal SalePrice { get; set; }

        public int ItemVariantId { get; set; }
        public int CreatedByUserAccountId { get; set; }
    }

    public class PriceHistoryUpdateDTO
    {
        public string? Description { get; set; }
        public decimal Cost { get; set; }
        public decimal SalePrice { get; set; }
    }

    public class ListPriceHistoryDTO
    {
        public int ItemVariantId { get; set; }
        public string ItemName { get; set; } = null!;
        public string VariantName { get; set; } = null!;
        public decimal SalePrice { get; set; }
    }

    public class ListCostHistoryDTO
    {
        public int ItemVariantId { get; set; }
        public string ItemName { get; set; } = null!;
        public string VariantName { get; set; } = null!;
        public decimal Cost { get; set; }
    }
    public class ListAllPriceHistoryDTO
    {
        public DateOnly Date { get; set; }
        public string? Description { get; set; }
        public decimal Cost { get; set; }
        public decimal SalePrice { get; set; }

        public string ItemName { get; set; } = null!;
        public string VariantName { get; set; } = null!;
        public string CreatedBy { get; set; } = null!;
    }
    public class ListCostByUserDTO
    {
        public string UserName { get; set; } = null!;
        public string ItemName { get; set; } = null!;
        public string VariantName { get; set; } = null!;
        public decimal Cost { get; set; }
    }

    public class ListPriceByUserDTO
    {
        public string UserName { get; set; } = null!;
        public string ItemName { get; set; } = null!;
        public string VariantName { get; set; } = null!;
        public decimal SalePrice { get; set; }
    }





}
