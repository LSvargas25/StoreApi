using System;
using System.Collections.Generic;

namespace StoreApi.Models;

public partial class Payment
{
    public int PaymentId { get; set; }

    public DateTime Date { get; set; }

    public decimal TotalAmount { get; set; }

    public bool IsCreditPayment { get; set; }

    public int? CreatedByUserId { get; set; }

    public int? SupplierId { get; set; }

    public int? CustomerId { get; set; }

    public int? PurchaseId { get; set; }

    public int? InvoiceId { get; set; }

    public virtual UserAccount? CreatedByUser { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual Invoice? Invoice { get; set; }

    public virtual ICollection<PaymentDetail> PaymentDetails { get; set; } = new List<PaymentDetail>();

    public virtual Purchase? Purchase { get; set; }

    public virtual Supplier? Supplier { get; set; }
}
