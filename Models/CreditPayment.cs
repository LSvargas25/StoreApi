using System;
using System.Collections.Generic;

namespace StoreApi.Models;

public partial class CreditPayment
{
    public int CreditPaymentId { get; set; }

    public decimal Amount { get; set; }

    public DateTime Date { get; set; }

    public int? CreatedByUserId { get; set; }

    public int CreditId { get; set; }

    public virtual UserAccount? CreatedByUser { get; set; }

    public virtual CreditAccount Credit { get; set; } = null!;
}
