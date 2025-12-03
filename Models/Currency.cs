using System;
using System.Collections.Generic;

namespace StoreApi.Models;

public partial class Currency
{
    public int CurrencyId { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public bool IsDefault { get; set; }

    public virtual ICollection<ExchangeRate> ExchangeRates { get; set; } = new List<ExchangeRate>();

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    public virtual ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();

    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();
}
