using System;
using System.Collections.Generic;

namespace StoreApi.Models;

public partial class Attribute
{
    public int AttributeId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<AttributeDetail> AttributeDetails { get; set; } = new List<AttributeDetail>();
}
