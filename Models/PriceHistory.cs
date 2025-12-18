using StoreApi.Models;

public partial class PriceHistory
{
    public int PriceHistoryId { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public string? Description { get; set; }

    public decimal Cost { get; set; }
    public decimal SalePrice { get; set; }

    // FK correcta
    public int ItemVariantId { get; set; }
    public int CreatedByUserAccountId { get; set; }

    // Navegaciones
    public virtual ItemVariant ItemVariant { get; set; } = null!;
    public virtual UserAccount CreatedByUserAccount { get; set; } = null!;
}

