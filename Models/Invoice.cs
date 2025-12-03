using System;
using System.Collections.Generic;

namespace StoreApi.Models;

public partial class Invoice
{
    public int InvoiceId { get; set; }

    public decimal Total { get; set; }

    public int CurrencyId { get; set; }

    public decimal ExchangeRate { get; set; }

    public bool IsActive { get; set; }

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public int? InvoiceTypeId { get; set; }

    public virtual Currency Currency { get; set; } = null!;

    public virtual ICollection<InvoiceDetail> InvoiceDetails { get; set; } = new List<InvoiceDetail>();

    public virtual InvoiceType? InvoiceType { get; set; }

    public virtual ICollection<InvoiceVersion> InvoiceVersions { get; set; } = new List<InvoiceVersion>();

    public virtual ICollection<OutboxMessage> OutboxMessages { get; set; } = new List<OutboxMessage>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();

    public virtual ICollection<WebhookEvent> WebhookEvents { get; set; } = new List<WebhookEvent>();
}
