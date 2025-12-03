using System;
using System.Collections.Generic;

namespace StoreApi.Models;

public partial class ItemImage
{
    public int ItemImageId { get; set; }

    public string Url { get; set; } = null!;

    public bool IsPrimary { get; set; }

    public int ItemId { get; set; }

    public virtual Item Item { get; set; } = null!;
}
