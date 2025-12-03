using System;
using System.Collections.Generic;

namespace StoreApi.Models;

public partial class PurchaseDetail
{
    public int PurchaseDetailId { get; set; }

    public int PurchaseId { get; set; }

    public int ItemVariantId { get; set; }

    public decimal Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public int? DiscountId { get; set; }

    public int? TaxId { get; set; }

    public decimal TotalLine { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Discount? Discount { get; set; }

    public virtual ItemVariant ItemVariant { get; set; } = null!;

    public virtual Purchase Purchase { get; set; } = null!;

    public virtual ICollection<PurchaseDetailTax> PurchaseDetailTaxes { get; set; } = new List<PurchaseDetailTax>();

    public virtual Tax? Tax { get; set; }
}
