using System;
using System.Collections.Generic;

namespace StoreApi.Models;

public partial class Tax
{
    public int TaxId { get; set; }

    public string Name { get; set; } = null!;

    public decimal Percentage { get; set; }

    public virtual ICollection<InvoiceDetailTax> InvoiceDetailTaxes { get; set; } = new List<InvoiceDetailTax>();

    public virtual ICollection<InvoiceDetail> InvoiceDetails { get; set; } = new List<InvoiceDetail>();

    public virtual ICollection<PurchaseDetailTax> PurchaseDetailTaxes { get; set; } = new List<PurchaseDetailTax>();

    public virtual ICollection<PurchaseDetail> PurchaseDetails { get; set; } = new List<PurchaseDetail>();
}
