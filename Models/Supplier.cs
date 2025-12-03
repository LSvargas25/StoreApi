using System;
using System.Collections.Generic;

namespace StoreApi.Models;

public partial class Supplier
{
    public int SupplierId { get; set; }

    public string Name { get; set; } = null!;

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public bool IsActive { get; set; }

    public int? SupplierTypeId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();

    public virtual SupplierType? SupplierType { get; set; }
}
