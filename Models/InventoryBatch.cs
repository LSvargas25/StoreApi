using System;
using System.Collections.Generic;

namespace StoreApi.Models;

public partial class InventoryBatch
{
    public int InventoryBatchId { get; set; }

    public string BatchNumber { get; set; } = null!;

    public int ItemVariantId { get; set; }

    public int WarehouseId { get; set; }

    public int Quantity { get; set; }

    public DateOnly? ManufactureDate { get; set; }

    public DateOnly? ExpirationDate { get; set; }

    public string? SupplierLotNumber { get; set; }

    public virtual ICollection<InventoryBatchMovement> InventoryBatchMovements { get; set; } = new List<InventoryBatchMovement>();

    public virtual ICollection<InvoiceDetail> InvoiceDetails { get; set; } = new List<InvoiceDetail>();

    public virtual ItemVariant ItemVariant { get; set; } = null!;

    public virtual Warehouse Warehouse { get; set; } = null!;
}
