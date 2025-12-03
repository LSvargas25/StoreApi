using System;
using System.Collections.Generic;

namespace StoreApi.Models;

public partial class VwSalesSummary
{
    public int InvoiceId { get; set; }

    public DateTime CreatedAt { get; set; }

    public decimal Total { get; set; }

    public int? CustomerId { get; set; }

    public string? CustomerName { get; set; }
}
