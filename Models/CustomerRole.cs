using System;
using System.Collections.Generic;

namespace StoreApi.Models;

public partial class CustomerRole
{
    public int CustomerRoleId { get; set; }

    public string Name { get; set; } = null!;

    public bool IsActive { get; set; }

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();
}
