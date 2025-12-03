using System;
using System.Collections.Generic;

namespace StoreApi.Models;

public partial class SupplierType
{
    public int SupplierTypeId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Supplier> Suppliers { get; set; } = new List<Supplier>();
}
