using System;
using System.Collections.Generic;

namespace StoreApi.Models;

public partial class CostingMethod
{
    public int CostingMethodId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<ItemCostHistory> ItemCostHistories { get; set; } = new List<ItemCostHistory>();
}
