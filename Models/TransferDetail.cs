using System;
using System.Collections.Generic;

namespace StoreApi.Models;

public partial class TransferDetail
{
    public int TransferDetailId { get; set; }

    public int TransferId { get; set; }

    public int ItemVariantId { get; set; }

    public int Quantity { get; set; }

    public virtual ItemVariant ItemVariant { get; set; } = null!;

    public virtual Transfer Transfer { get; set; } = null!;
}
