using System;
using System.Collections.Generic;

namespace StoreApi.Models;

public partial class Warehouse
{
    public int WarehouseId { get; set; }

    public string Name { get; set; } = null!;

    public string? Address { get; set; }

    public string? PhoneNumber { get; set; }

    public bool IsActive { get; set; }

    public DateOnly CreatedAt { get; set; }
     

    public virtual ICollection<CashRegister> CashRegisters { get; set; } = new List<CashRegister>();

    public virtual ICollection<InventoryBatch> InventoryBatches { get; set; } = new List<InventoryBatch>();

    public virtual ICollection<InventoryLedger> InventoryLedgers { get; set; } = new List<InventoryLedger>();

    public virtual ICollection<InventoryWarehouse> InventoryWarehouses { get; set; } = new List<InventoryWarehouse>();

    public virtual ICollection<Transfer> TransferFromWarehouses { get; set; } = new List<Transfer>();

    public virtual ICollection<Transfer> TransferToWarehouses { get; set; } = new List<Transfer>();
}
