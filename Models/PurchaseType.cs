using System;
using System.Collections.Generic;

namespace StoreApi.Models;

public partial class PurchaseType
{
    public int PurchaseTypeId { get; set; }

    public string Name { get; set; } = null!;

    public bool IsActive { get; set; }

    public virtual ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();
}
