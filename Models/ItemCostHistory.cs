using System;
using System.Collections.Generic;

namespace StoreApi.Models;

public partial class ItemCostHistory
{
    public int ItemCostHistoryId { get; set; }

    public int ItemVariantId { get; set; }

    public int MethodId { get; set; }

    public decimal? OldCost { get; set; }

    public decimal? NewCost { get; set; }

    public string? Reason { get; set; }

    public DateTime ChangedAt { get; set; }

    public int? ChangedByUserId { get; set; }

    public virtual ItemVariant ItemVariant { get; set; } = null!;

    public virtual CostingMethod Method { get; set; } = null!;
}
