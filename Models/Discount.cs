using System;
using System.Collections.Generic;

namespace StoreApi.Models;

public partial class Discount
{
    public int DiscountId { get; set; }

    public string Name { get; set; } = null!;

    public decimal Percentage { get; set; }

    public virtual ICollection<InvoiceDetail> InvoiceDetails { get; set; } = new List<InvoiceDetail>();

    public virtual ICollection<PurchaseDetail> PurchaseDetails { get; set; } = new List<PurchaseDetail>();
}
