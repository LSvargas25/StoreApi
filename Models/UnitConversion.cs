using System;
using System.Collections.Generic;

namespace StoreApi.Models;

public partial class UnitConversion
{
    public int UnitConversionId { get; set; }

    public int FromUnitId { get; set; }

    public int ToUnitId { get; set; }

    public decimal Factor { get; set; }

    public virtual Unit FromUnit { get; set; } = null!;

    public virtual Unit ToUnit { get; set; } = null!;
}
