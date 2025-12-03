using System;
using System.Collections.Generic;

namespace StoreApi.Models;

public partial class InvoiceType
{
    public int InvoiceTypeId { get; set; }

    public string Name { get; set; } = null!;

    public bool IsActive { get; set; }

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
}
