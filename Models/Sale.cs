using System;
using System.Collections.Generic;

namespace StoreApi.Models;

public partial class Sale
{
    public int SaleId { get; set; }

    public DateTime SaleDate { get; set; }

    public string? Notes { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? CreatedByUserId { get; set; }

    public int? CustomerId { get; set; }

    public int? InvoiceId { get; set; }

    public int CurrencyId { get; set; }

    public virtual UserAccount? CreatedByUser { get; set; }

    public virtual Currency Currency { get; set; } = null!;

    public virtual Customer? Customer { get; set; }

    public virtual Invoice? Invoice { get; set; }

    public virtual ICollection<OutboxMessage> OutboxMessages { get; set; } = new List<OutboxMessage>();

    public virtual ICollection<WebhookEvent> WebhookEvents { get; set; } = new List<WebhookEvent>();
}
