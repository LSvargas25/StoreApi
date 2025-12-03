using System;
using System.Collections.Generic;

namespace StoreApi.Models;

public partial class PaymentDetail
{
    public int PaymentDetailId { get; set; }

    public string? Reference { get; set; }

    public decimal Amount { get; set; }

    public int PaymentId { get; set; }

    public virtual Payment Payment { get; set; } = null!;
}
