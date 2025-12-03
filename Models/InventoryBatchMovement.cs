using System;
using System.Collections.Generic;

namespace StoreApi.Models;

public partial class InventoryBatchMovement
{
    public int InventoryBatchMovementId { get; set; }

    public int InventoryBatchId { get; set; }

    public int Quantity { get; set; }

    public string? MovementType { get; set; }

    public string? Reference { get; set; }

    public DateTime Date { get; set; }

    public virtual InventoryBatch InventoryBatch { get; set; } = null!;
}
