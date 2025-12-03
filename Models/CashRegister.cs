using System;
using System.Collections.Generic;

namespace StoreApi.Models;

public partial class CashRegister
{
    public int CashRegisterId { get; set; }

    public string Name { get; set; } = null!;

    public bool IsActive { get; set; }

    public int? WarehouseId { get; set; }

    public virtual ICollection<CashRegisterSession> CashRegisterSessions { get; set; } = new List<CashRegisterSession>();

    public virtual Warehouse? Warehouse { get; set; }
}
