using System;
using System.Collections.Generic;

namespace StoreApi.Models;

public partial class Transfer
{
    public int TransferId { get; set; }

    public int FromWarehouseId { get; set; }

    public int ToWarehouseId { get; set; }

    public DateTime Date { get; set; }

    public bool IsActive { get; set; }

    public int? CreatedByUserId { get; set; }

    public string? Description { get; set; }

    public virtual UserAccount? CreatedByUser { get; set; }

    public virtual Warehouse FromWarehouse { get; set; } = null!;

    public virtual Warehouse ToWarehouse { get; set; } = null!;

    public virtual ICollection<TransferDetail> TransferDetails { get; set; } = new List<TransferDetail>();
}
