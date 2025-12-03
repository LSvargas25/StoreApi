using System;
using System.Collections.Generic;

namespace StoreApi.Models;

public partial class CreditAccount
{
    public int CreditId { get; set; }

    public decimal TotalCredit { get; set; }

    public decimal CurrentBalance { get; set; }

    public bool IsActive { get; set; }

    public int CustomerId { get; set; }

    public virtual ICollection<CreditPayment> CreditPayments { get; set; } = new List<CreditPayment>();

    public virtual Customer Customer { get; set; } = null!;
}
