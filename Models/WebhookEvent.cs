using System;
using System.Collections.Generic;

namespace StoreApi.Models;

public partial class WebhookEvent
{
    public int WebhookEventId { get; set; }

    public int OutboxMessageId { get; set; }

    public int SubscriberId { get; set; }

    public string Status { get; set; } = null!;

    public DateTime? LastAttemptAt { get; set; }

    public int? InvoiceId { get; set; }

    public int? SaleId { get; set; }

    public virtual Invoice? Invoice { get; set; }

    public virtual OutboxMessage OutboxMessage { get; set; } = null!;

    public virtual Sale? Sale { get; set; }

    public virtual WebhookSubscriber Subscriber { get; set; } = null!;
}
