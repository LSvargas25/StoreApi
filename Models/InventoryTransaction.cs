using System;
using System.Collections.Generic;

namespace StoreApi.Models;

public partial class InventoryTransaction
{
    public int InventoryTransactionId { get; set; }

    public decimal Amount { get; set; }

    public int Quantity { get; set; }

    public DateTime Date { get; set; }

    public bool IsActive { get; set; }

    public int TransactionTypeId { get; set; }

    public int InventoryWarehouseId { get; set; }

    public int? CreatedByUserId { get; set; }

    public string? Notes { get; set; }

    public virtual UserAccount? CreatedByUser { get; set; }

    public virtual ICollection<InventoryLedger> InventoryLedgers { get; set; } = new List<InventoryLedger>();

    public virtual InventoryWarehouse InventoryWarehouse { get; set; } = null!;

    public virtual ICollection<InvoiceDetail> InvoiceDetails { get; set; } = new List<InvoiceDetail>();

    public virtual TransactionType TransactionType { get; set; } = null!;
}
