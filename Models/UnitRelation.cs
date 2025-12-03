using System;
using System.Collections.Generic;

namespace StoreApi.Models;

public partial class UnitRelation
{
    public int UnitRelationId { get; set; }

    public int UnitId { get; set; }

    public int ItemVariantId { get; set; }

    public string Value { get; set; } = null!;

    public bool IsFavorite { get; set; }

    public virtual ItemVariant ItemVariant { get; set; } = null!;

    public virtual Unit Unit { get; set; } = null!;
}
