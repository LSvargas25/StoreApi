using System;
using System.Collections.Generic;

namespace StoreApi.Models;

public partial class WebhookSubscriber
{
    public int WebhookSubscriberId { get; set; }

    public string Url { get; set; } = null!;

    public string? Events { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<WebhookEvent> WebhookEvents { get; set; } = new List<WebhookEvent>();
}
