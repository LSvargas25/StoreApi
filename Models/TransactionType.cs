using System;
using System.Collections.Generic;

namespace StoreApi.Models;

public partial class TransactionType
{
    public int TransactionTypeId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<InventoryTransaction> InventoryTransactions { get; set; } = new List<InventoryTransaction>();
}
