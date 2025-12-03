using System;
using System.Collections.Generic;

namespace StoreApi.Models;

public partial class CashRegisterSession
{
    public int CashRegisterSessionId { get; set; }

    public int CashRegisterId { get; set; }

    public DateTime OpenDate { get; set; }

    public decimal OpenAmount { get; set; }

    public DateTime? CloseDate { get; set; }

    public decimal? CloseAmount { get; set; }

    public bool IsActive { get; set; }

    public int? CreatedByUserId { get; set; }

    public virtual CashRegister CashRegister { get; set; } = null!;

    public virtual UserAccount? CreatedByUser { get; set; }
}
