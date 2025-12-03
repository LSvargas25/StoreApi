using System;
using System.Collections.Generic;

namespace StoreApi.Models;

public partial class OutboxMessage
{
    public int OutboxMessageId { get; set; }

    public string EventType { get; set; } = null!;

    public string? Payload { get; set; }

    public string Status { get; set; } = null!;

    public int Retries { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? InvoiceId { get; set; }

    public int? SaleId { get; set; }

    public virtual Invoice? Invoice { get; set; }

    public virtual Sale? Sale { get; set; }

    public virtual ICollection<WebhookEvent> WebhookEvents { get; set; } = new List<WebhookEvent>();
}
