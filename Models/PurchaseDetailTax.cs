using System;
using System.Collections.Generic;

namespace StoreApi.Models;

public partial class PurchaseDetailTax
{
    public int PurchaseDetailTaxId { get; set; }

    public int PurchaseDetailId { get; set; }

    public int TaxId { get; set; }

    public decimal Amount { get; set; }

    public virtual PurchaseDetail PurchaseDetail { get; set; } = null!;

    public virtual Tax Tax { get; set; } = null!;
}
