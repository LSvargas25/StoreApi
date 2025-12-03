using System;
using System.Collections.Generic;

namespace StoreApi.Models;

public partial class Unit
{
    public int UnitId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<UnitConversion> UnitConversionFromUnits { get; set; } = new List<UnitConversion>();

    public virtual ICollection<UnitConversion> UnitConversionToUnits { get; set; } = new List<UnitConversion>();

    public virtual ICollection<UnitRelation> UnitRelations { get; set; } = new List<UnitRelation>();
}
