using System;
using System.Collections.Generic;

namespace StoreApi.Models;

public partial class InventoryLedger
{
    public int LedgerId { get; set; }

    public int InventoryTransactionId { get; set; }

    public int ItemVariantId { get; set; }

    public int WarehouseId { get; set; }

    public int OldQty { get; set; }

    public int NewQty { get; set; }

    public int QtyChange { get; set; }

    public decimal Cost { get; set; }

    public int RunningBalanceQty { get; set; }

    public decimal RunningBalanceCost { get; set; }

    public string? Method { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual InventoryTransaction InventoryTransaction { get; set; } = null!;

    public virtual ItemVariant ItemVariant { get; set; } = null!;

    public virtual Warehouse Warehouse { get; set; } = null!;
}
