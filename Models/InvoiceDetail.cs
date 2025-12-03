using System;
using System.Collections.Generic;

namespace StoreApi.Models;

public partial class InvoiceDetail
{
    public int InvoiceDetailId { get; set; }

    public int InvoiceId { get; set; }

    public DateTime Date { get; set; }

    public decimal TaxAmount { get; set; }

    public decimal DiscountAmount { get; set; }

    public decimal TotalLine { get; set; }

    public decimal Total { get; set; }

    public int? DiscountId { get; set; }

    public int? TaxId { get; set; }

    public decimal CostAtMovement { get; set; }

    public int? InventoryTransactionId { get; set; }

    public int? InventoryBatchId { get; set; }

    public int ItemVariantId { get; set; }

    public virtual Discount? Discount { get; set; }

    public virtual InventoryBatch? InventoryBatch { get; set; }

    public virtual InventoryTransaction? InventoryTransaction { get; set; }

    public virtual Invoice Invoice { get; set; } = null!;

    public virtual ICollection<InvoiceDetailTax> InvoiceDetailTaxes { get; set; } = new List<InvoiceDetailTax>();

    public virtual ItemVariant ItemVariant { get; set; } = null!;

    public virtual Tax? Tax { get; set; }
}
