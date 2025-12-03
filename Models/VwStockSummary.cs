using System;
using System.Collections.Generic;

namespace StoreApi.Models;

public partial class VwStockSummary
{
    public int ItemVariantId { get; set; }

    public string VariantName { get; set; } = null!;

    public int WarehouseId { get; set; }

    public string WarehouseName { get; set; } = null!;

    public int ActualStock { get; set; }
}
