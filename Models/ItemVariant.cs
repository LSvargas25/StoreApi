using System;
using System.Collections.Generic;

namespace StoreApi.Models;

public partial class ItemVariant
{
    public int ItemVariantId { get; set; }

    public string Name { get; set; } = null!;

    public string? Sku { get; set; }

    public decimal StandardPrice { get; set; }

    public decimal StandardCost { get; set; }

    public string? Barcode { get; set; }

    public bool IsActive { get; set; }

    public int ItemId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<InventoryBatch> InventoryBatches { get; set; } = new List<InventoryBatch>();

    public virtual ICollection<InventoryLedger> InventoryLedgers { get; set; } = new List<InventoryLedger>();

    public virtual ICollection<InventoryWarehouse> InventoryWarehouses { get; set; } = new List<InventoryWarehouse>();

    public virtual ICollection<InvoiceDetail> InvoiceDetails { get; set; } = new List<InvoiceDetail>();

    public virtual Item Item { get; set; } = null!;

    public virtual ICollection<ItemCostHistory> ItemCostHistories { get; set; } = new List<ItemCostHistory>();

    public virtual ICollection<PurchaseDetail> PurchaseDetails { get; set; } = new List<PurchaseDetail>();

    public virtual ICollection<TransferDetail> TransferDetails { get; set; } = new List<TransferDetail>();

    public virtual ICollection<UnitRelation> UnitRelations { get; set; } = new List<UnitRelation>();

    public ICollection<PriceHistory> PriceHistories { get; set; } = new List<PriceHistory>();
}
