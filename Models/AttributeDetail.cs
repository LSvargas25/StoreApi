using System;
using System.Collections.Generic;

namespace StoreApi.Models;

public partial class AttributeDetail
{
    public int ItemId { get; set; }

    public int AttributeId { get; set; }

    public string Value { get; set; } = null!;

    public bool IsFavorite { get; set; }

    public virtual Attribute Attribute { get; set; } = null!;

    public virtual Item Item { get; set; } = null!;
}
