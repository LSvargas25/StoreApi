using System;
using System.Collections.Generic;

namespace StoreApi.Models;

public partial class Purchase
{
    public int PurchaseId { get; set; }

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public bool IsActive { get; set; }

    public decimal Total { get; set; }

    public int CurrencyId { get; set; }

    public decimal ExchangeRate { get; set; }

    public int? PurchaseTypeId { get; set; }

    public int? CreatedByUserId { get; set; }

    public int? SupplierId { get; set; }

    public virtual UserAccount? CreatedByUser { get; set; }

    public virtual Currency Currency { get; set; } = null!;

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ICollection<PurchaseDetail> PurchaseDetails { get; set; } = new List<PurchaseDetail>();

    public virtual PurchaseType? PurchaseType { get; set; }

    public virtual Supplier? Supplier { get; set; }
}
