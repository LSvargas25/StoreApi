using System;
using System.Collections.Generic;

namespace StoreApi.Models;

public partial class PriceHistory
{
    public int PriceHistoryId { get; set; }

    public DateTime Date { get; set; }

    public string? Description { get; set; }

    public decimal Cost { get; set; }

    public decimal SalePrice { get; set; }

    public int ItemId { get; set; }

    public virtual Item Item { get; set; } = null!;
}
