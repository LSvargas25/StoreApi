using System;
using System.Collections.Generic;

namespace StoreApi.Models;

public partial class InvoiceVersion
{
    public int InvoiceVersionId { get; set; }

    public int InvoiceId { get; set; }

    public int VersionNumber { get; set; }

    public string DataSnapshot { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public int? CreatedByUserId { get; set; }

    public virtual Invoice Invoice { get; set; } = null!;
}
