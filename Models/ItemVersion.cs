using System;
using System.Collections.Generic;

namespace StoreApi.Models;

public partial class ItemVersion
{
    public int ItemVersionId { get; set; }

    public int ItemId { get; set; }

    public int VersionNumber { get; set; }

    public string DataSnapshot { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public int? CreatedByUserId { get; set; }

    public virtual Item Item { get; set; } = null!;
}
