using System;
using System.Collections.Generic;

namespace StoreApi.Models;

public partial class InventoryWarehouse
{
    public int InventoryWarehouseId { get; set; }

    public bool IsActive { get; set; }

    public int ActualStock { get; set; }

    public int MinStock { get; set; }

    public int MaxStock { get; set; }

    public int WarehouseId { get; set; }

    public int ItemVariantId { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<InventoryTransaction> InventoryTransactions { get; set; } = new List<InventoryTransaction>();

    public virtual ItemVariant ItemVariant { get; set; } = null!;

    public virtual Warehouse Warehouse { get; set; } = null!;
}
