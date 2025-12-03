using System;
using System.Collections.Generic;

namespace StoreApi.Models;

public partial class ItemCategory
{
    public int ItemCategoryId { get; set; }

    public string Name { get; set; } = null!;

    public bool IsActive { get; set; }

    public virtual ICollection<Item> Items { get; set; } = new List<Item>();
}
