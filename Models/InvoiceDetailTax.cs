using System;
using System.Collections.Generic;

namespace StoreApi.Models;

public partial class InvoiceDetailTax
{
    public int InvoiceDetailTaxId { get; set; }

    public int InvoiceDetailId { get; set; }

    public int TaxId { get; set; }

    public decimal Amount { get; set; }

    public virtual InvoiceDetail InvoiceDetail { get; set; } = null!;

    public virtual Tax Tax { get; set; } = null!;
}
